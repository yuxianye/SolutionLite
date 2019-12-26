using Assimp;
using Desktop.Core;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Animations;
using HelixToolkit.Wpf.SharpDX.Assimp;
using HelixToolkit.Wpf.SharpDX.Controls;
using HelixToolkit.Wpf.SharpDX.Model;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Models;
using Prism.Commands;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Utility.Windows;

namespace Desktop.ThreeDModule.ViewModels
{
    /// <summary>
    /// 用户管理VM
    /// </summary>
    public class ThreeDxViewModel : Desktop.Core.PageableViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ThreeDxViewModel()
        {
            init();

        }


        private void init()
        {
            initCommand();
            initMainContent();
            //initCommand();
            //initMenus();
            // on camera changed callback
            //CameraModelChanged += (s, e) =>
            //{
            //    if (cameraModel == Orthographic)
            //    {
            //        if (!(Camera is OrthographicCamera))
            //            Camera = defaultOrthographicCamera;
            //    }
            //    else if (cameraModel == Perspective)
            //    {
            //        if (!(Camera is PerspectiveCamera))
            //            Camera = defaultPerspectiveCamera;
            //    }
            //    else
            //    {
            //        throw new HelixToolkitException("Camera Model Error.");
            //    }
            //};
            //Camera = new PerspectiveCamera() { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 200), LookDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, -200), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), FarPlaneDistance = 1000 };

        }

        #region 命令定义和初始化

        /// <summary>
        /// 打开文件命令
        /// </summary>
        public DelegateCommand OpenFileCommand { get; set; }

        ///// <summary>
        ///// 3D控件左击
        ///// </summary>
        //public HelixToolkit.Wpf.PointSelectionCommand PointSelectionCommand { get; private set; }

        /// <summary>
        /// 默认视角命令
        /// </summary>
        public DelegateCommand DefaultViewPositionCommand { get; set; }

        /// <summary>
        /// 初始化界面相关的命令
        /// </summary>
        private void initCommand()
        {
            this.OpenFileCommand = new DelegateCommand(OnExecuteOpenFileCommand);
            this.DefaultViewPositionCommand = new DelegateCommand(OnExecuteDefaultViewPositionCommand);
            //this.PointSelectionCommand = new HelixToolkit.Wpf.PointSelectionCommand(MainContent, OnExecuteModelsPointSelectionCommand, OnExecuteVisualsPointSelectionCommand);

            HitLineGeometry.Positions = new Vector3Collection(2);
            HitLineGeometry.Positions.Add(SharpDX.Vector3.Zero);
            HitLineGeometry.Positions.Add(SharpDX.Vector3.Zero);
            HitLineGeometry.Indices = new IntCollection(2);
            HitLineGeometry.Indices.Add(0);
            HitLineGeometry.Indices.Add(1);
        }

        #endregion

        //private Viewport3DX.
        /// <summary>
        /// 初始化3D视窗。增加灯光、默认摄像机等公共对象
        /// </summary>
        private void initMainContent()
        {
            mainContent.ZoomExtentsWhenLoaded = true;
            ////Background = Utility.Windows.ResourceHelper.FindResource("V3DBackgroundBrush") as Brush,
            mainContent.ShowCoordinateSystem = true;
            mainContent.ShowViewCube = true;

            mainContent.EffectsManager = effectsManager;
            //添加灯光
            mainContent.Items.Add(new DirectionalLight3D() { Direction = new System.Windows.Media.Media3D.Vector3D(-1, -1, -1) });
            mainContent.Items.Add(new AmbientLight3D() { Color = Color.FromArgb(255, 50, 50, 50) });
            //地板坐标
            //mainContent.Items.Add(new AxisPlaneGridModel3D() { AutoSpacing = true, RenderShadowMap = false }); ;

            mainContent.Items.Add(element3DPresenter);
            //mainContent.InputBindings.Add(new MouseBinding(this.PointSelectionCommand, new MouseGesture(MouseAction.LeftClick)));

            mainContent.AddHandler(Element3D.MouseDown3DEvent, new RoutedEventHandler((s, e) =>
            {
                var arg = e as MouseDown3DEventArgs;

                if (arg.HitTestResult == null)
                {
                    return;
                }
                var meshNode = arg.HitTestResult.ModelHit as MeshNode;
                //meshNode.RenderWireframe = !meshNode.RenderWireframe;
                //meshNode.WireframeColor = new SharpDX.Color4(200, 200, 200, 200);
                Random rnd = new Random();
                meshNode.Material = materials[rnd.Next(0, materials.Count - 1)];

                //if (arg.HitTestResult.ModelHit is InstancingMeshGeometryModel3D)
                //{

                //    //var index = (int)hit.Tag;
                //    //InstanceParam[index].EmissiveColor = InstanceParam[index].EmissiveColor != Colors.Yellow.ToColor4() ? Colors.Yellow.ToColor4() : Colors.Black.ToColor4();
                //    //InstanceParam = (InstanceParameter[])InstanceParam.Clone();
                //    //break;
                //}
                //else if (arg.HitTestResult.ModelHit is LineGeometryModel3D)
                //{
                //    //var index = (int)hit.Tag;
                //    //SelectedLineInstances = new Matrix[] { ModelInstances[index] };
                //    //break;
                //}




                if (arg.HitTestResult.ModelHit is SceneNode node && node.Tag is AttachedNodeViewModel vm)
                {
                    vm.Selected = !vm.Selected;



                }
            }));

            element3DPresenter.Content = sceneNodeGroupModel3D;
            //mainContent.Camera.CreateLeftHandSystem = true;

            //添加灯光
            //mainContent.Children.Add(new HelixToolkit.Wpf.SunLight());

            //保存默认相机对象，供恢复默认视角使用
            //defaultCamera = this.MainContent.Camera.Clone() as HelixToolkit.Wpf.SharpDX.Camera;
            //this.MainContent.Camera = defaultCamera;
            // mainContent.ZoomExtents();

            // sceneNodeGroup = new SceneNodeGroupModel3D();
            //mainContent.Items.Add(sceneNodeGroup);
            //var container = new ContainerUIElement3D();
            //var element = new ModelUIElement3D();
            //var geometry = new GeometryModel3D();
            //var meshBuilder = new MeshBuilder();
            //meshBuilder.AddSphere(new Point3D(0, 0, 0), 2, 100, 50);
            //geometry.Geometry = meshBuilder.ToMesh();
            //geometry.Material = Materials.Green;
            //element.Model = geometry;
            //element.Transform = new TranslateTransform3D(5, 0, 0);
            ////element.MouseDown += this.ContainerElementMouseDown;
            //container.Children.Add(element);
            //mainContent.Children.Add(container);
            //BoxVisual3D box = new BoxVisual3D();
            //box.Center = new Point3D(20, 20, 20);
            //mainContent.Children.Add(box);
            //mainContent.InputBindings.Add(new MouseBinding(this.PointSelectionCommand, new MouseGesture(MouseAction.LeftClick)));

            //UserControls.AgvUIElement3D agvUIElement3D = new UserControls.AgvUIElement3D();
            //mainContent.Children.Add(agvUIElement3D);
        }

        private PhongMaterialCollection materials = new PhongMaterialCollection();

        private void Element3DPresenter_Mouse3DDown(object sender, MouseDown3DEventArgs e)
        {
            SelectedGeometry = e.HitTestResult.Geometry;

        }

        public const string Orthographic = "Orthographic Camera";

        public const string Perspective = "Perspective Camera";

        private SceneNodeGroupModel3D sceneNodeGroupModel3D = new SceneNodeGroupModel3D();
        private Element3DPresenter element3DPresenter = new Element3DPresenter();
        private HelixToolkitScene scene;

        private IEffectsManager effectsManager = new DefaultEffectsManager();
        //public IEffectsManager EffectsManager
        //{
        //    get { return effectsManager; }
        //    protected set
        //    {
        //        SetProperty(ref effectsManager, value);
        //    }
        //}


        private bool isLoading = false;
        public bool IsLoading
        {
            private set => SetProperty(ref isLoading, value);
            get => isLoading;
        }


        private string cameraModel;
        public string CameraModel
        {
            get
            {
                return cameraModel;
            }
            set
            {
                if (SetProperty(ref cameraModel, value))
                {
                    OnCameraModelChanged();
                }
            }
        }

        public event EventHandler CameraModelChanged;

        protected OrthographicCamera defaultOrthographicCamera = new OrthographicCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 1, FarPlaneDistance = 100 };

        protected PerspectiveCamera defaultPerspectiveCamera = new PerspectiveCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 0.5, FarPlaneDistance = 150 };

        private HelixToolkit.Wpf.SharpDX.Camera camera;
        public HelixToolkit.Wpf.SharpDX.Camera Camera
        {
            get
            {
                return camera;
            }

            protected set
            {
                SetProperty(ref camera, value);
                CameraModel = value is PerspectiveCamera
                                       ? Perspective
                                       : value is OrthographicCamera ? Orthographic : null;
            }
        }
        /// <summary>
        /// 默认相机，恢复默认视角使用
        /// </summary>
        private HelixToolkit.Wpf.SharpDX.Camera defaultCamera;

        #region  MainContent Viewport3DX 定义初始化
        private Viewport3DX mainContent = new Viewport3DX()
        {
            //Background = Utility.Windows.ResourceHelper.FindResource("V3DBackgroundBrush") as Brush
            Background = Utility.Windows.ResourceHelper.FindResource("ControlBackgroundBrush") as Brush
        };

        /// <summary>
        /// 主控件
        /// </summary>
        public Viewport3DX MainContent
        {
            get { return mainContent; }
            set { SetProperty(ref mainContent, value); }
        }

        #endregion 



        #region 命令和消息等执行函数

        private void OnExecuteOpenFileCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = $"{HelixToolkit.Wpf.SharpDX.Assimp.Importer.SupportedFormatsString}";
            ;
            if (dialog.ShowDialog() == true)
            {
                //init3DModels(dialog.FileNames.ToList());
                foreach (var path in dialog.FileNames)
                {
                    Task.Run(() =>
                    {
                        var loader = new Importer();
                        return loader.Load(path);
                    }).ContinueWith((result) =>
                    {
                        IsLoading = false;
                        if (result.IsCompleted)
                        {
                            scene = result.Result;
                            //Animations.Clear();
                            //GroupModel.Clear();
                            if (scene != null)
                            {
                                //if (scene.Root != null)
                                //{
                                //    foreach (var node in scene.Root.Traverse())
                                //    {
                                //        if (node is MaterialGeometryNode m)
                                //        {
                                //            if (m.Material is PBRMaterialCore pbr)
                                //            {
                                //                pbr.RenderEnvironmentMap = RenderEnvironmentMap;
                                //            }
                                //            else if (m.Material is PhongMaterialCore phong)
                                //            {
                                //                phong.RenderEnvironmentMap = RenderEnvironmentMap;
                                //            }
                                //        }
                                //    }
                                //}
                                //scene.Root.MouseDown += Root_MouseDown;
                                sceneNodeGroupModel3D.AddNode(scene.Root);
                                foreach (var node in scene.Root.Items.Traverse(false))
                                {
                                    if (node is BoneSkinMeshNode m)
                                    {
                                        if (!m.IsSkeletonNode)
                                        {
                                            m.IsThrowingShadow = true;
                                            m.WireframeColor = new SharpDX.Color4(0, 0, 1, 1);
                                            boneSkinNodes.Add(m);
                                            m.MouseDown += M_MouseDown;
                                        }
                                        else
                                        {
                                            skeletonNodes.Add(m);
                                            m.Visible = false;
                                        }
                                    }
                                }
                                //sceneNodeGroupModel3D.MouseDown += SceneNodeGroupModel3D_MouseDown3D;
                                //if (scene.HasAnimation)
                                //{
                                //    foreach (var ani in scene.Animations)
                                //    {
                                //        Animations.Add(ani);
                                //    }
                                //}
                                foreach (var n in scene.Root.Traverse())
                                {
                                    n.Tag = new AttachedNodeViewModel(n);
                                }
                            }
                        }
                        else if (result.IsFaulted && result.Exception != null)
                        {
                            MessageBox.Show(result.Exception.Message);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    //sceneNodeGroupModel3D.AddNode();
                }
            }
            mainContent.ZoomExtents();
            //defaultCamera = mainContent.Camera.Clone ();
            defaultCamera = this.MainContent.Camera.Clone() as HelixToolkit.Wpf.SharpDX.Camera;
            //sceneNodeGroupModel3D.Mouse3DDown += SceneNodeGroupModel3D_Mouse3DDown;

        }

        //private void SceneNodeGroupModel3D_Mouse3DDown(object sender, MouseDown3DEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    SelectedGeometry = e.HitTestResult.Geometry;


        //}

        private List<BoneSkinMeshNode> boneSkinNodes = new List<BoneSkinMeshNode>();
        private List<BoneSkinMeshNode> skeletonNodes = new List<BoneSkinMeshNode>();

        private void M_MouseDown(object sender, SceneNodeMouseDownArgs e)
        {

            //e.HitResult.Geometry.Colors = new Color4Collection(50);
            var v = e.HitResult.Geometry;

            //public Material SelectedMaterial { get; } = new PhongMaterial() { EmissiveColor = Color.Yellow };

            var result = e.HitResult;

            //e.Source.Material= new PhongMaterial() { EmissiveColor = Color.Yellow };
            // var vv = (e.Source as MeshGeometryModel3D);
            //e.Source.Material = new PhongMaterial() { EmissiveColor = Color.Yellow };


            HitLineGeometry.Positions[0] = result.PointHit - result.NormalAtHit * 0.5f;
            HitLineGeometry.Positions[1] = result.PointHit + result.NormalAtHit * 0.5f;
            HitLineGeometry.UpdateVertices();

            //viewModel.SelectedGeometry = e.HitTestResult.Geometry;

        }
        public LineGeometry3D HitLineGeometry { get; } = new LineGeometry3D() { IsDynamic = true };

        private IList<BatchedMeshGeometryConfig> batchedMeshes;
        public IList<BatchedMeshGeometryConfig> BatchedMeshes
        {
            set
            {
                SetProperty(ref batchedMeshes, value);
            }
            get
            {
                return batchedMeshes;
            }
        }
        public System.Windows.Media.Media3D.Transform3D BatchedTransform
        {
            get;
        } = new System.Windows.Media.Media3D.ScaleTransform3D(0.1, 0.1, 0.1);

        private Geometry3D selectedGeometry;
        public Geometry3D SelectedGeometry
        {
            set
            {
                if (SetProperty(ref selectedGeometry, value))
                {
                    SelectedTransform = new System.Windows.Media.Media3D.MatrixTransform3D(BatchedMeshes.Where(x => x.Geometry == value).Select(x => x.ModelTransform).First().ToMatrix3D() * BatchedTransform.Value);
                }
            }
            get { return selectedGeometry; }
        }

        private System.Windows.Media.Media3D.Transform3D selectedTransform;
        public System.Windows.Media.Media3D.Transform3D SelectedTransform
        {
            set
            {
                SetProperty(ref selectedTransform, value);
            }
            get { return selectedTransform; }
        }

        /// <summary>
        /// 执行恢复默视角命令，恢复相机位置和缩放到填充模式
        /// </summary>
        private void OnExecuteDefaultViewPositionCommand()
        {
            //Task.Factory.StartNew(new Action(init));
            //OnExecuteSizeChangedCommand();
            this.mainContent.ZoomExtents();
            this.MainContent.Camera = defaultCamera.Clone() as HelixToolkit.Wpf.SharpDX.Camera;
            //this.MainContent.CameraController..LookDirection = defaultCameraLookDirection;
        }

        #endregion



        protected virtual void OnCameraModelChanged()
        {
            var eh = CameraModelChanged;
            if (eh != null)
            {
                eh(this, new EventArgs());
            }
        }




        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Disposing()
        {
            //释放相关的资源

            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }

    }




}
