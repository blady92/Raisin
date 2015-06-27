using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using Cyber.Audio;
using Cyber.AudioEngine;
using Cyber.CAdditionalLibs;
using Cyber.CItems;
using Cyber.CItems.CStaticItem;
using Cyber.CollisionEngine;
using Cyber.GraphicsEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Cyber.CLogicEngine;
using Cyber.CItems.CDynamicItem;
using Cyber.CLevel;
using Color = Microsoft.Xna.Framework.Color;

namespace Cyber.CGameStateEngine
{
    public class GameStateMainGame : GameState
    {
        private int i = 0;
        private int angle = 0;
        private float value = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public ContentManager theContentManager { get; set; }

        public bool endGame { get; set; } 
        public bool lostGame { get; set; }
        public bool firstStart { get; set; }

        public LevelStage level { get; set; }

        private KeyboardState oldState;
        private KeyboardState newState;
        private AudioController audio;

        public AudioController Audio
        {
            get { return audio; }
            set { audio = value; }
        }

        //2D elements
        private ConsoleSprites console;

        //3D elements
        public StaticItem samanthaGhostController { get; set; }
        public DynamicItem samanthaActualPlayer { get; set; }
        public DynamicItem samanthaActualPlayerCopy { get; set; }
        public DynamicItem samanthaActualPlayerRun { get; set; }

        public DynamicItem terminalActualModel { get; set; }
        public DynamicItem gateActualModel { get; set; }
        private ColliderController colliderController;
        // TODO: Refactor na private
        public List<StaticItem> npcBillboardsList { get; set; }

        private float przesuniecie;
        
        //Plot elements
        private IDGenerator generatedID;
        public PlotTwistClass plot { get; set; }

        //Barriers for clock manipulation
        Boolean addPushed = false;
        Boolean subPushed = false;

        //Spojrzenie postaci tam gdzie zwrot
        float rotateSam = 0.0f;
        Matrix samPointingAtDirection = Matrix.CreateRotationX(MathHelper.ToRadians(90.0f)) * Matrix.Identity * Matrix.CreateRotationZ(MathHelper.ToRadians(0));
        bool changedDirection = false;

        public bool escaped;
        public float podjazdStopPoint { get; set; }
        public float podjazdBefore { get; set; }

        //Effect shader
        Texture2D m_texture;
        Texture2D m_texture_wall;
        Texture2D m_texture_concave;
        Texture2D m_texture_convex;
        Texture2D m_texture_column;
        Texture2D m_texture_floor_alert;
        Effect celShader;
        Effect celShaderDynamic;
        Texture2D celMap;
        Texture2D celMapLight;
        Vector4 lightDirection = new Vector4(-0.3333333f, 0.6666667f, 0.6666667f, 0.0f);
        //shader outline
        Effect outlineShader;
        float defaultThickness = 0.20f;
        float defaultThreshold = 0.20f;
        float outlineThickness = 0.5f;
        float outlineThreshold = 0.47f;
        RenderTarget2D celTarget;
        StaticItem TerminalBillboardHelper;
        StaticItem gateBillboardHelper;

        //Terminal Animation
        AnimationPlayer terminalPlayer;
        AnimationClip terminalClip;
        KeyboardState OldKeyState;
        bool playTerminalAnimation;
        int clickedTab = 0;
        bool timeToHide = false;

        //Gate Animation
        AnimationPlayer gatePlayer;
        AnimationClip gateClip;
        float gateX = 600.0f;
        float gateY = 810.0f;
        float gateZ = -292.0f;

        //Variables for shrinking view position
        //Min and moax describes vertex of 2D box by its diagonal
        private SceneSplitter sceneSplitter;
        private PointF rangeMin;
        private PointF rangeMax;

        //AudioEffects
        private AudioModel audioModel =  new AudioModel("CyberBank");
        private AudioController audioController;
        bool terminalOpenPlayed = false;
        bool terminalClosePlayed = false;
        bool gateOpeningPlayed = false;
        bool walkingPlayed = false;
        bool samIsWalking = false;
        bool clickedPositivePlayed = false;
        bool alerted = false;
        bool alertSystemPlayed = false;
        
        //Radary
        private StaticItem radar;
        private float opacityOfRadar = 0.4f;
        bool systemIsAlerted = false;
    

        #region ShadowMapping
        //ShadowMapping
        const int shadowMapWidthHeight = 2048;
        BoundingFrustum cameraFrustum = new BoundingFrustum(Matrix.Identity);
        Vector3 lightDir = new Vector3(-0.3333333f, 0.6666667f, 0.6666667f);

        Model gridModel;
        Model dudeModel;

        Matrix lightViewProjection;
        Matrix stageTerminalView;
        float terminalWoop = 0.0f;

        RenderTarget2D shadowRenderTarget;

        // END OF SHADOW MAPPING
        #endregion

        public void Unload()
        {
            theContentManager.Unload();
        }
        
        public void LoadContent(ContentManager theContentManager, GraphicsDevice device)
        {
            //ShadowMap
            gridModel = theContentManager.Load<Model>("Assets/grid");
            dudeModel = theContentManager.Load<Model>("Assets/3D/Interior/Interior_Terminal");
            shadowRenderTarget = new RenderTarget2D(device, shadowMapWidthHeight, shadowMapWidthHeight, false, SurfaceFormat.Single, DepthFormat.Depth24);
            
            endGame = false;
            lostGame = false;

            audioController = new AudioController(audioModel);
            audioController.setAudio();
            
            if (plot == null || firstStart)
            {
                plot = new PlotTwistClass();
                firstStart = false;
            }

            #region Load Dialogs
            if (!plot.loaded)
            {
                plot.Initialize();
            }
            #endregion

            this.theContentManager = theContentManager;

            #region Load Shaders
            //CellShading
            celShader = theContentManager.Load<Effect>("Assets/ShadersFX/CelShader");
            celShaderDynamic = theContentManager.Load<Effect>("Assets/ShadersFX/CelShaderDynamic");
            m_texture = theContentManager.Load<Texture2D>("Assets/3D/Interior/Textures/terminalUVv1");
            m_texture_wall = theContentManager.Load<Texture2D>("Assets/3D/Interior/Textures/TexScianaB");
            m_texture_concave = theContentManager.Load<Texture2D>("Assets/3D/Interior/Textures/TexConcave");
            m_texture_convex = theContentManager.Load<Texture2D>("Assets/3D/Interior/Textures/TexConvex");
            m_texture_column = theContentManager.Load<Texture2D>("Assets/3D/Interior/Textures/TexWierza");
            m_texture_floor_alert = theContentManager.Load<Texture2D>("Assets/3D/Interior/Textures/TexPodloga_Alert");
            celMap = theContentManager.Load<Texture2D>("Assets/3D/Interior/Textures/celMap");
            celMapLight = theContentManager.Load<Texture2D>("Assets/3D/Interior/Textures/celMapLight");
            celShader.Parameters["LightDirection"].SetValue(lightDirection);
            celShaderDynamic.Parameters["LightDirection"].SetValue(lightDirection);
            celShader.Parameters["ColorMap"].SetValue(m_texture);
            celShaderDynamic.Parameters["ColorMap"].SetValue(m_texture);
            celShader.Parameters["CelMap"].SetValue(celMap);
            celShaderDynamic.Parameters["CelMap"].SetValue(celMap);
            //Outline
            outlineShader = theContentManager.Load<Effect>("Assets/ShadersFX/OutlineShader");
            outlineShader.Parameters["Thickness"].SetValue(outlineThickness);
            outlineShader.Parameters["Threshold"].SetValue(outlineThreshold);
            outlineShader.Parameters["ScreenSize"].SetValue(new Vector2(device.Viewport.Bounds.Width, device.Viewport.Bounds.Height));

            celTarget = new RenderTarget2D(device, device.Viewport.Width, device.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            #endregion
            #region Load 2D elements
            console = new ConsoleSprites(this, audio);
            console.plotAction = plot;
            console.LoadContent(theContentManager);
            #endregion
            #region Samantha's animation 
            samanthaGhostController = new StaticItem("Assets/3D/Characters/Ally_Bunker");
            samanthaGhostController.LoadItem(theContentManager);
            samanthaGhostController.Type = StaticItemType.samantha;

            samanthaActualPlayer = new DynamicItem("Assets//3D/Characters/sammy_idle2", "Anim1", new Vector3(100, 100, 50));

            samanthaActualPlayer.LoadItem(theContentManager);
            samanthaActualPlayer.Type = DynamicItemType.samantha;

            samanthaActualPlayerCopy = new DynamicItem("Assets//3D/Characters/sammy_idle2", "Anim1", new Vector3(100, 100, 50));

            samanthaActualPlayerCopy.LoadItem(theContentManager);
            samanthaActualPlayerCopy.Type = DynamicItemType.samantha;


            samanthaActualPlayerRun = new DynamicItem("Assets//3D/Characters/sammy_running2", "Anim1", new Vector3(100, 100, 50));

            samanthaActualPlayerRun.LoadItem(theContentManager);
            samanthaActualPlayerRun.Type = DynamicItemType.samantha;
            #endregion
            #region Terminal's animations
            terminalActualModel = new DynamicItem("Assets//3D/Interior/terminal_animated_inv", "Take 001", new Vector3(100, 100, 50));
            terminalActualModel.LoadItem(theContentManager);
            terminalActualModel.Type = DynamicItemType.none;

            terminalPlayer = terminalActualModel.SkinnedModel.AnimationPlayer;
            terminalClip = terminalActualModel.SkinnedModel.Clip;


            gateActualModel = new DynamicItem("Assets//3D/Interior/Interior_Gate_AnimBigger", "Take 001", new Vector3(100, 100, 50));
            gateActualModel.LoadItem(theContentManager);
            gateActualModel.Type = DynamicItemType.none;

            gatePlayer = gateActualModel.SkinnedModel.AnimationPlayer;
            gateClip = gateActualModel.SkinnedModel.Clip;


            #endregion

            level.LoadContent(device);
        }

        public void LookAtSam(ref Vector3 cameraTarget)
        {
            cameraTarget.X = -samanthaGhostController.Position.X;
            cameraTarget.Y = samanthaGhostController.Position.Y;
        }

        public Vector3 returnSamanthaPosition()
        {
            return samanthaGhostController.Position;
        }

        public void SetUpScene(GraphicsDevice device)
        {
            escaped = false;
            
            #region Setting plot
            plot.SamChecked = false;
            lostGame = false;
            endGame = false;
            #endregion
            #region setups data initials
            int i = 0;
            float mnoznikPrzesuniecaSciany = 19.5f;
            float mnoznikPrzesunieciaOther = mnoznikPrzesuniecaSciany;
            float terminalZ = 50.0f;
            float objectZ = 0.0f;
            float wallOffset = 9.75f;
            float cornerOffset = 5.5f;
            #endregion

            npcBillboardsList = new List<StaticItem>();
            level.SetUpScene(device);

            #region Setting 'em all to colliders
            colliderController = new ColliderController(console);
            colliderController.samantha = samanthaGhostController;
            //colliderController.staticItemList = stageElements;
            colliderController.staticItemList = level.ConnectedColliders;
            colliderController.npcItem = level.npcList;
            colliderController.plot = plot;
            colliderController.exit = level.escapeCollider;
            #endregion
            #region Setting scene splitter points
            sceneSplitter = new SceneSplitter(samanthaGhostController, new PointF(550.0f, 600.0f));
            #endregion
            #region Sort elements by position
            level.StageElements = new List<StaticItem>(level.StageElements.OrderBy(p => p.Position.X).ThenBy(q => q.Position.Y));
            #endregion
            #region Initialize AI
            AI ai = AI.Instance;
            ai.ColliderController = colliderController;
            ai.FreeSpaceMap = StageUtils.RoomListToFreeSpaceMap(level.Stage.Rooms);
            #endregion
        }

        #region Setup clock
        public void SetUpClock()
        {
            //Clock clock = Clock.Instance;
            //clock.RemainingSeconds = /*4 * 60*/20;
            //clock.AddEvent(Clock.BEFOREOVER, 0, TimePassed);
            //clock.Pause();
        }
        #endregion 

        private void TimePassed(object sender, int time)
        {
            Debug.WriteLine("TIMEOUT");
        }


        public override void Draw(GraphicsDevice device, SpriteBatch spriteBatch, 
            GameTime gameTime, Matrix world, Matrix view, Matrix projection,
            ref float cameraRotation
            )
        {

          

            //ShadowMap <ODKOMENTOWAC W CELU CIENIOWANIA>
            //lightViewProjection = CreateLightViewProjectionMatrix();
            //device.BlendState = BlendState.Opaque;
            //device.DepthStencilState = DepthStencilState.Default;
            //CreateShadowMap(device, world, view, projection);
            //DrawWithShadowMap(world, view, projection, device);
           
            #region Before billboards
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.SamplerStates[0] = SamplerState.LinearWrap;

            celShader.Parameters["Projection"].SetValue(projection);
            celShader.Parameters["View"].SetValue(view);

            device.SetRenderTarget(celTarget);
            device.Clear(Color.Black);

            //ShadowMap<ODKOMENTOWAC W CELU CIENIOWANIA>
                      // DrawWithShadowMap(stageTerminalView, view, projection, device);
           
            Matrix samanthaGhostView = Matrix.Identity *
                           Matrix.CreateRotationZ(MathHelper.ToRadians(angle)) *
                           Matrix.CreateTranslation(samanthaGhostController.Position);

            Matrix samanthaActualPlayerView = Matrix.CreateRotationY(MathHelper.ToRadians(rotateSam)) * samPointingAtDirection * Matrix.CreateTranslation(samanthaGhostController.Position);
            samanthaActualPlayer.DrawItem(gameTime, device, samanthaActualPlayerView, view, projection);
          
         
           // device.SetRenderTarget(null);
            
            //Matrix samanthaColliderView = Matrix.CreateTranslation(samanthaGhostController.ColliderInternal.Position);
            //samanthaGhostController.DrawItem(device, samanthaGhostView, view, projection);
            //samanthaActualPlayer.DrawItem(device, samanthaActualPlayerView, view, projection, celShaderDynamic);
            Matrix samanthaColliderView = Matrix.CreateTranslation(samanthaGhostController.ColliderInternal.Position);
            //samanthaGhostController.DrawItem(device, samanthaGhostView, view, projection);
            samanthaActualPlayer.DrawItem(device, samanthaActualPlayerView, view, projection, celShaderDynamic);
            //samanthaGhostController.ColliderInternal.DrawBouding(device, samanthaColliderView, view, projection);
            //samanthaGhostController.ColliderExternal.DrawBouding(device, samanthaColliderView, view, projection);
            //samanthaGhostController.DrawRadar(samanthaGhostController.Position, samanthaGhostController.moveColliderExternal, 2f, 1f, device, view, projection);
            Matrix podjazdModel = Matrix.CreateTranslation(level.podjazd.Position);
            level.podjazd.DrawItem(device, podjazdModel, view, projection);

            //samanthaGhostController.DrawItem(device, samanthaGhostView, view, projection);
            //Matrix samanthaColliderView = Matrix.CreateTranslation(samanthaGhostController.ColliderInternal.Position);
            //samanthaGhostController.ColliderInternal.DrawBouding(device, samanthaColliderView, view, projection);
            //samanthaGhostController.ColliderExternal.DrawBouding(device, samanthaColliderView, view, projection);

            //Matrix radarModel = Matrix.CreateTranslation(samanthaGhostController.Position + new Vector3(samanthaGhostController.moveColliderExternal.X, samanthaGhostController.moveColliderExternal.Y, 0)*2f);
            //Matrix radarCollider = Matrix.CreateTranslation(samanthaGhostController.ColliderExternal.Position);
            //radar.DrawItem(device, radarModel, view, projection);
            //radar.ColliderInternal.DrawBouding(device, radarCollider, view, projection);

            #endregion
            #region Rysowanie elementów sceny

            //foreach (var gateHolder in gateList)
            //{
            //    if (gateHolder.Collider != null)
            //    {
            //        gateHolder.Collider.DrawBouding(device, Matrix.CreateTranslation(gateHolder.Collider.Position), view, projection);
            //    }
            //}
           
            

            foreach (StaticItem stageElement in level.StageElements)
            {
                if(sceneSplitter.IsItemWithin(stageElement)){
                    Matrix stageElementView = Matrix.Identity *
                        Matrix.CreateRotationZ(MathHelper.ToRadians(stageElement.Rotation)) *
                        Matrix.CreateTranslation(stageElement.Position);
                    //if (stageElement.Type == StaticItemType.terminal)
                    //{
                    //    Matrix terminalColliderView = Matrix.CreateTranslation(stageElement.ColliderInternal.Position);
                    //    stageElement.ColliderInternal.DrawBouding(device, terminalColliderView, view, projection);
                    //}
                    if (stageElement.Type != StaticItemType.teleporter) {
                        if ((stageElement.Type == StaticItemType.terminal))
                        {
                        
                        stageElementView = Matrix.CreateRotationX(MathHelper.ToRadians(90.0f)) * stageElementView * Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, -50.0f));
                        //ShadowMap <ODKOMENTOWAC W CELU CIENIOWANIA>
                        //stageTerminalView = stageElementView;
                        //stageTerminalView = Matrix.CreateRotationX(MathHelper.ToRadians(-89.0f)) * stageTerminalView * Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, 80.0f));

                       terminalActualModel.DrawItem(gameTime, device, stageElementView, view, projection);
                       TerminalBillboardHelper = stageElement;
                       // stageElement.DrawOnlyTab(device, view, projection, cameraRotation);
                        //stageElement.DrawItem(device, stageElementView, view, projection);
                    }
                    else if(stageElement.Type == StaticItemType.gate)
                    {
                        stageElementView = stageElementView * Matrix.CreateRotationX(MathHelper.ToRadians(90.0f)) * Matrix.CreateTranslation(new Vector3(541.0f, 762.0f, -322.0f)) *Matrix.CreateScale(0.43f, 0.43f, 0.43f);
                        gateActualModel.DrawItem(gameTime, device, stageElementView, view, projection);

                        gateBillboardHelper = stageElement;
                                               
                    }
                    else if (stageElement.Type == StaticItemType.wall)
                    {
                        celShader.Parameters["CelMap"].SetValue(celMap);
                        celShader.Parameters["ColorMap"].SetValue(m_texture_wall);
                        stageElement.DrawItem(device, stageElementView, view, projection, celShader);
                    }
                    else if (stageElement.Type == StaticItemType.concave)
                    {
                        celShader.Parameters["CelMap"].SetValue(celMap);
                        celShader.Parameters["ColorMap"].SetValue(m_texture_concave);
                        stageElement.DrawItem(device, stageElementView, view, projection, celShader);
                    }
                    else if (stageElement.Type == StaticItemType.convex)
                    {
                        celShader.Parameters["CelMap"].SetValue(celMap);
                        celShader.Parameters["ColorMap"].SetValue(m_texture_convex);
                        stageElement.DrawItem(device, stageElementView, view, projection, celShader);
                    }
                    else if (stageElement.Type == StaticItemType.column)
                    {
                        celShader.Parameters["CelMap"].SetValue(celMap);
                        celShader.Parameters["ColorMap"].SetValue(m_texture_column);
                        stageElement.DrawItem(device, stageElementView, view, projection, celShader);
                    }
                    else if (stageElement.Type == StaticItemType.oxygenGenerator)
                    {
                        stageElement.DrawItem(device, stageElementView, view, projection);
                    }
                    else if (stageElement.Type == StaticItemType.floor)
                    {
                       

                        if(systemIsAlerted)
                        {
                            if (!alertSystemPlayed)
                            {
                                audioController.alertSystemController("Play");
                                //  audioController.alertSystemController("Pause");
                                alertSystemPlayed = true;
                            }

                            celShader.Parameters["CelMap"].SetValue(celMapLight);
                            celShader.Parameters["ColorMap"].SetValue(m_texture_floor_alert);
                            stageElement.DrawItem(device, stageElementView, view, projection, celShader);
                            audioController.alertSystemController("Resume");

                            
                        }
                        else
                        {
                            celShader.Parameters["CelMap"].SetValue(celMap);
                            stageElement.DrawItem(device, stageElementView, view, projection);
                           
                            if(alertSystemPlayed)
                            {
                                audioController.alertSystemController("Pause");
                            }
                           
                        }
                        
                    }
                    else
                    { 
                      if (stageElement.OnOffBilboard)
                       {
                         stageElement.DrawItem(device, stageElementView, view, projection, cameraRotation);
                       }
                       else
                      {
                          stageElement.DrawItem(device, stageElementView, view, projection);
                          if (stageElement.particles != null)
                              {
                                    stageElement.particles.Update();
                                    stageElement.particles.Draw(device, view, projection, cameraRotation, stageElement.Position);
                                }
                                stageElement.DrawItem(device, stageElementView, view, projection, cameraRotation);
                            }
                        } //end else
                    }
                }
            }
            
            level.escapeemitter.Draw(device, view, projection, cameraRotation, new Vector3(0, 0, 0));
            if(!plot.GeneratorFound)
                level.generatorParticles.Draw(device, view, projection, cameraRotation, new Vector3(0, 0, 0));

            #endregion
            #region Rysowanie NPCów
            foreach (StaticItem item in level.npcList)
            {
                Matrix stageElementView = Matrix.Identity *
                                          Matrix.CreateRotationZ(MathHelper.ToRadians(item.Rotation)) *
                                          Matrix.CreateTranslation(item.Position);

                item.DrawItem(device, stageElementView, view, projection);
                //if (item.Type == StaticItemType.spy)
                //{
                //    Matrix itemColliderView = Matrix.CreateTranslation(item.ColliderExternal.Position);
                //    item.ColliderExternal.DrawBouding(device, itemColliderView, view, projection);
                //}
                if (item.Type == StaticItemType.flyer)
                {
                    item.DrawRadar(item.Position + new Vector3(0, -20, 0), item.moveColliderExternal, 2f, 0.5f, device, view, projection, opacityOfRadar);
                }
                else if (item.Type == StaticItemType.tank)
                {
                    item.DrawRadar(item.Position + new Vector3(20, 20, 0), item.moveColliderExternal, 2f, 1.5f, device, view, projection, opacityOfRadar);
                }
                else if (item.Type == StaticItemType.spy)
                {
                    item.DrawRadar(item.Position + new Vector3(-35, -12, 0), item.moveColliderExternal, 2f, 1.2f, device, view, projection, opacityOfRadar);
                }

                if (item.particles != null)
                {
                    item.particles.Update();
                    item.particles.Draw(device, view, projection, cameraRotation, item.Position);
                }
                item.DrawItem(device, stageElementView, view, projection, cameraRotation);

             
            }
            #endregion


            device.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, outlineShader);
            spriteBatch.Draw(celTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            //Drawing billboards
            TerminalBillboardHelper.DrawOnlyTab(device, view, projection, cameraRotation);
            level.escapeCollider.DrawOnlyEscapeBilboard(device, view, projection, cameraRotation);
            if (!plot.Gate1Opened && level.level == Level.level2)
            {
                gateBillboardHelper.DrawOnlyBillboardGate(device, view, projection, cameraRotation);
            }
            foreach (StaticItem item in npcBillboardsList)
            {
                item.DrawOnlyBillboardGate(device, view, projection, cameraRotation);
            }
          

            console.Draw(spriteBatch);
            #region Draw Colliders for static Items
            //foreach (var collider in ConnectedColliders)
            //{
            //    Matrix ColliderTest = Matrix.CreateTranslation(collider.ColliderInternal.Position);
            //    collider.ColliderInternal.DrawBouding(device, ColliderTest, view, projection);
            //}
            #endregion
        }

   
        public override void Update(GraphicsDevice device, GameTime gameTime, KeyboardState currentKeyboardState, MouseState currentMouseState, ref float cameraArc, ref float cameraRotation, ref float cameraDistance, ref Vector3 cameraTarget, ref float cameraZoom)
        {

            outlineShader.Parameters["Thickness"].SetValue(outlineThickness);
            outlineShader.Parameters["Threshold"].SetValue(outlineThreshold);

            if (level.SamanthaGhostController.ColliderExternal.AABB.Intersects(level.escapeCollider.ColliderInternal.AABB))
            {
                if (plot.PossibleEscape)
                {
                    samIsWalking = false;
                    base.State = GameState.States.loadingGame;
                    escaped = true;
                }
            }

            level.escapeemitter.Update();
            console.Update();
            KeyboardState newState = currentKeyboardState;

            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            samanthaGhostController.SkinnedModel.UpdateCamera(device, gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance);
            samanthaActualPlayer.SkinnedModel.UpdateCamera(device, gameTime, currentKeyboardState, currentMouseState, ref cameraArc, ref cameraRotation, ref cameraDistance);
            samanthaActualPlayer.SkinnedModel.UpdatePlayer(gameTime);

            if (!plot.GeneratorFound)
                level.generatorParticles.Update();

            if (!walkingPlayed)
            {
                audio.walkingController("Play");
                walkingPlayed = true;

            }
            if (!samIsWalking)
            {
                audio.walkingController("Pause");
            }
            else if (samIsWalking)
            {
                audio.walkingController("Resume");
            }

            terminalPlayer.Update(new TimeSpan(0, 0, 0), true, Matrix.Identity);

            if(plot.Gate1Opened)
            {
                if (!gateOpeningPlayed)
                {
                    audioController.gateOpeningController("Play");
                    gateOpeningPlayed = true;
                } 

               if(gatePlayer.CurrentKeyFrame < 240)
               {
                   gatePlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
               }
               else
               {
                   gatePlayer.Update(new TimeSpan(0, 0, 0), true, Matrix.Identity);
               }
                
            }
            else
            {
                gatePlayer.Update(new TimeSpan(0, 0, 0), true, Matrix.Identity);
            }

            #region Animacja Terminala
            KeyboardState NewKeyState = Keyboard.GetState();

            if (NewKeyState.IsKeyDown(Keys.F2) && OldKeyState.IsKeyUp(Keys.F2))
            {
                outlineThreshold += 0.01f;
                Debug.WriteLine("TRESZOLD: " + outlineThreshold);

            }
            if (NewKeyState.IsKeyDown(Keys.F3) && OldKeyState.IsKeyUp(Keys.F3))
            {
                outlineThreshold -= 0.01f;
                Debug.WriteLine("TRESZOLD: " + outlineThreshold);
            }
            if (NewKeyState.IsKeyDown(Keys.F4) && OldKeyState.IsKeyUp(Keys.F4))
            {
                outlineThickness += 0.01f;
                Debug.WriteLine("FYKNES: " + outlineThickness);
            }
            if (NewKeyState.IsKeyDown(Keys.F5) && OldKeyState.IsKeyUp(Keys.F5))
            {
                outlineThickness -= 0.01f;
                Debug.WriteLine("FYKNES: " + outlineThickness);
            }


            if(console.IsUsed)
            {
                if (NewKeyState.IsKeyDown(Keys.Enter) && OldKeyState.IsKeyUp(Keys.Enter))
                {
                    if(!clickedPositivePlayed)
                    {
                        audioController.clickedPositiveController("Play");
                        clickedPositivePlayed = true;
                    }
                   
                }
                else if (NewKeyState.IsKeyDown(Keys.Enter) && OldKeyState.IsKeyUp(Keys.Tab))
                {
                    clickedPositivePlayed = false;
                }

           
                if (NewKeyState.IsKeyDown(Keys.Tab) && OldKeyState.IsKeyUp(Keys.Tab) && (playTerminalAnimation == false) || playTerminalAnimation == false)
                {
                    clickedTab += 1;
                    playTerminalAnimation = true;
                    timeToHide = true;
                    terminalOpenPlayed = false;
                }
                else if (NewKeyState.IsKeyDown(Keys.Tab) && OldKeyState.IsKeyUp(Keys.Tab) && (playTerminalAnimation == true) && (terminalPlayer.CurrentKeyFrame == 160))
                {
                    playTerminalAnimation = false;
                }

                OldKeyState = NewKeyState;

                if (playTerminalAnimation && (clickedTab % 2 == 1))
                {
                    terminalPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
                  
                    if(!terminalOpenPlayed)
                    {
                        audioController.terminalSoundEffect("Play");
                        terminalOpenPlayed = true;
                    }

                    if (terminalPlayer.CurrentKeyFrame == 160)
                    {
                        playTerminalAnimation = false;
                    }

                }
                else
                {
                    terminalPlayer.Update(new TimeSpan(0, 0, 0), true, Matrix.Identity);
                }
            }
            else if (!console.IsUsed)
            {
                if (terminalPlayer.CurrentKeyFrame == 2)
                {
                    playTerminalAnimation = false;
                    timeToHide = false;
                    terminalClosePlayed = false;
                }
                else
                {
                    if (!terminalClosePlayed)
                    {
                        audioController.terminalSoundEffect("PlayInvert");
                        terminalClosePlayed = true;
                    }
                    terminalPlayer.Update(-gameTime.ElapsedGameTime, true, Matrix.Identity);
                }
            }
            #endregion
            #region Sterowanie zegarem
            //if (newState.IsKeyDown(Keys.T))
            //{
            //    if (Clock.Instance.CanResume())
            //    {
            //        Clock.Instance.Resume();
            //        Debug.WriteLine("Starting clock...");
            //    }
            //    if (newState.IsKeyDown(Keys.Add))
            //    {
            //        addPushed = true;
            //    }
            //    if (newState.IsKeyUp(Keys.Add) && addPushed)
            //    {
            //        addPushed = false;
            //        Clock.Instance.AddSeconds(60);
            //        Debug.WriteLine("ADDED +1");
            //    }
            //    if (newState.IsKeyDown(Keys.Subtract))
            //    {
            //        subPushed = true;
            //    }
            //    if (newState.IsKeyUp(Keys.Subtract) && subPushed)
            //    {
            //        subPushed = false;
            //        Clock.Instance.AddSeconds(-60);
            //        Debug.WriteLine("ADDED -1");
            //    }
            //}
            //if (newState.IsKeyDown(Keys.Y))
            //{
            //    if (Clock.Instance.CanPause())
            //    {
            //        Clock.Instance.Pause();
            //        Debug.WriteLine("Stopped clock");
            //    }
            //}
            #endregion
            #region Sterowanie Samanthą i kamerą
            Vector3 move = new Vector3(0, 0, 0);
            colliderController.PlayAudio = audio.Play0;
            if (!console.IsUsed)
            {
                samIsWalking = false;
                if (newState.IsKeyDown(Keys.S))
                {
                    samIsWalking = true;
                   
                    move = new Vector3(0, -1.5f, 0);
                    colliderController.CheckCollision(samanthaGhostController, move);
                    podjazdCollision();
                    cameraTarget.Y = samanthaGhostController.Position.Y;

                    samanthaActualPlayer = samanthaActualPlayerRun;
                    sceneSplitter.SetSplitterSceneView(samanthaGhostController);

                    //Debug.WriteLine("Rotate sam: " + rotateSam);
                    if (rotateSam >= -6.8f && rotateSam <= 180.0f)
                    {
                        rotateSam += time * 0.2f;
                    }
                    if (rotateSam > 180.0f && rotateSam <= 270.0f || rotateSam <= -86.0f)
                    {
                        rotateSam -= time * 0.2f;
                        if (rotateSam <= -180.0f)
                        {
                            rotateSam = 180.0f;
                        }
                    }
                }
                else samanthaActualPlayer = samanthaActualPlayerCopy;

                if (newState.IsKeyDown(Keys.W))
                {
                    samIsWalking = true;

                    move = new Vector3(0, 1.5f, 0);
                    colliderController.CheckCollision(samanthaGhostController, move);
                    podjazdCollision();
                    cameraTarget.Y = samanthaGhostController.Position.Y;

                    samanthaActualPlayer = samanthaActualPlayerRun;
                    sceneSplitter.SetSplitterSceneView(samanthaGhostController);

                    //Debug.WriteLine("Rotate sam: " + rotateSam);
                    if (rotateSam >= -179.9f && rotateSam < 0.0f || rotateSam > 180.0f)
                    {
                        rotateSam += time * 0.2f;
                        //  Debug.WriteLine("D wins");
                        if (rotateSam >= 360.0f)
                        {
                            rotateSam = 0.0f;
                        }
                    }
                    else if (rotateSam > 0.0f && rotateSam <= 180.0f)
                    {
                        rotateSam -= time * 0.2f;
                        //  Debug.WriteLine("A wins");
                    }

                }

                if (newState.IsKeyDown(Keys.A)) {

                    samIsWalking = true;

                    move = new Vector3(-1.5f, 0, 0);
                    colliderController.CheckCollision(samanthaGhostController, move);
                    podjazdCollision();
                    cameraTarget.X = -samanthaGhostController.Position.X;

                    samanthaActualPlayer = samanthaActualPlayerRun;
                    sceneSplitter.SetSplitterSceneView(samanthaGhostController);

                    //Debug.WriteLine("Rotate sam: " + rotateSam);
                    if (rotateSam >= -179.9f && rotateSam <= 90.0f)
                    {
                        rotateSam += time * 0.2f;
      
                    }
                    if (rotateSam <= 269.9f && rotateSam > 90.0f)
                    {
                        rotateSam -= time * 0.2f;
                    }
                  
                   // changedDirection = true;
                   // samPointingAtDirection = Matrix.CreateRotationY(MathHelper.ToRadians(rotateSam)) * samPointingAtDirection; 
                }
                if (newState.IsKeyDown(Keys.D))
                {
                    samIsWalking = true;

                    move = new Vector3(1.5f, 0, 0);
                    colliderController.CheckCollision(samanthaGhostController, move);
                    podjazdCollision();
                    cameraTarget.X = -samanthaGhostController.Position.X;

                    samanthaActualPlayer = samanthaActualPlayerRun;
                    sceneSplitter.SetSplitterSceneView(samanthaGhostController);

                    //Debug.WriteLine("Rotate sam: " + rotateSam);  
                    if ((rotateSam <= 90.0f) && (rotateSam > -90.0f))
                    {
                        rotateSam -= time * 0.2f;
                    }
                    if (rotateSam >= 170.0f || rotateSam <= -90.0f || (rotateSam > 90.0f && rotateSam < 170.0f))
                    {
                        rotateSam += time * 0.2f;
                        if(rotateSam > 270.0f)
                        {
                            rotateSam = -90.0f;
                        }
                    }
                }
              
            }
            else
            {
                console.Action();
            }
            #endregion
            #region Zoom kamery
            CameraZoom(colliderController.CallTerminalAfterCollision(samanthaGhostController), ref cameraZoom, 0.05f);
            #endregion
            #region AI
            if (colliderController.EnemyCollision(samanthaGhostController))
            {
                if(!alerted)
                {
                    audioController.alertSamController("Play");
                    alerted = true;
                    systemIsAlerted = true;
                }
              
                AI.Instance.AlertOthers(samanthaGhostController);
            }
            else
            {
                alerted = false;
                systemIsAlerted = false;
            }


            #endregion

            oldState = newState;
            AI.Instance.MoveNPCs(null);

            #region End game conditions
            KeyboardState first = Keyboard.GetState();
            KeyboardState second = new KeyboardState();
            if ((first.IsKeyDown(Keys.NumPad9) && second.IsKeyUp(Keys.NumPad9)) || plot.GeneratorOn)
            {
                Thread.Sleep(1500);
                endGame = true;
            }
            if (plot.SamChecked)
            {
                audioController.alertSystemController("Stop");
                lostGame = true;
                plot.Gate1Opened = false;
            }            
            #endregion
            #region Teleporting Sam near to generator
            if (first.IsKeyDown(Keys.F1) && second.IsKeyUp(Keys.F1) && level.level == Level.level2)
            {
                Vector3 replace = new Vector3(1501.5f, 702.0f, 0);
                samanthaActualPlayer.Position = replace;
                samanthaActualPlayerCopy.Position = replace;
                samanthaActualPlayerRun.Position = replace;
                samanthaGhostController.Position = replace;
                samanthaGhostController.FixColliderInternal(new Vector3(0.75f, 0.75f, 1f), new Vector3(-15f, -15f, 10f));
                samanthaGhostController.FixColliderExternal(new Vector3(1.25f, 1.25f, 1.25f), new Vector3(-25f, -25f, 20f));
                sceneSplitter.SetSplitterSceneView(samanthaGhostController);
                plot.TeleportToGenerator();
            }
            second = first;
            #endregion
        }

        #region Wejście i zejście (poprawione, kurde bele)
        public void podjazdCollision()
        {
            if (level.podjazd.ColliderInternal.AABB.Intersects(samanthaGhostController.ColliderInternal.AABB))
            {
                if (podjazdBefore < podjazdStopPoint - samanthaGhostController.Position.X)
                {
                    samanthaGhostController.Position += new Vector3(0, 0, 0.6f);
                }
                else if (podjazdBefore > podjazdStopPoint - samanthaGhostController.Position.X)
                {
                    samanthaGhostController.Position += new Vector3(0, 0, -0.6f);
                }
                podjazdBefore = podjazdStopPoint - samanthaGhostController.Position.X;
            }
        }
        #endregion 
        #region Funkcje do zoomowania kamery
        public void CameraZoom(bool collision, ref float actualPosition, float speed)
        {
            if(collision)
                CameraZoomIn(ref actualPosition, speed);
            else
                CameraZoomOut(ref actualPosition, speed);
        }

        public void CameraZoomIn(ref float actualPosition, float speed)
        {
            if (actualPosition < 2.75f)
                actualPosition += speed;
        }

        public void CameraZoomOut(ref float actualPosition, float speed)
        {
            if (actualPosition > 1.6f)
                actualPosition -= speed;
        }
        #endregion
        #region Złączenie kolizji
        public BoundingBox JoinToFirstCollider(BoundingBox box1, BoundingBox box2)
        {
            return BoundingBox.CreateMerged(box1, box2);
        }
        #endregion

        private Matrix CreateLightViewProjectionMatrix()
        {
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero, -lightDir, Vector3.Up);
            Vector3[] frustumCorners = cameraFrustum.GetCorners();

            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - lightDir,
                                                   Vector3.Up);

            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            return lightView * lightProjection;
        }

        void CreateShadowMap(GraphicsDevice device, Matrix world, Matrix view, Matrix projection)
        {
            device.SetRenderTarget(shadowRenderTarget);
            device.Clear(Color.White);

            DrawModel(dudeModel, true, world, view, projection);

            device.SetRenderTarget(null);
        }

        void DrawModel(Model model, bool createShadowMap, Matrix world, Matrix view, Matrix projection)
        {
            string techniqueName = createShadowMap ? "CreateShadowMap" : "DrawWithShadowMap";

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    // Set the currest values for the effect
                    effect.CurrentTechnique = effect.Techniques[techniqueName];
                    effect.Parameters["World"].SetValue(world);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    effect.Parameters["LightDirection"].SetValue(lightDir);
                    effect.Parameters["LightViewProj"].SetValue(lightViewProjection);

                    if (!createShadowMap)
                        effect.Parameters["ShadowMap"].SetValue(shadowRenderTarget);

                }
                // Draw the mesh
                mesh.Draw();
            }
        }

        void DrawWithShadowMap(Matrix world, Matrix view, Matrix projection, GraphicsDevice device)
        {
            device.Clear(Color.Black);

            device.SamplerStates[1] = SamplerState.PointClamp;
            // Draw the grid
            DrawModel(gridModel, false, world*Matrix.CreateRotationX(MathHelper.ToRadians(90.0f))*Matrix.CreateTranslation(new Vector3(0.0f,333.0f, -256.0f)), view, projection);

            // Draw the dude model
            DrawModel(dudeModel, false, world, view, projection);
        }

    }
}