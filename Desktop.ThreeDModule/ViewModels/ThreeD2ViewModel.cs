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
using System.Windows.Media.Media3D;
using Utility.Windows;

namespace Desktop.ThreeDModule.ViewModels
{
    /// <summary>
    /// 用户管理VM
    /// </summary>
    public class ThreeD2ViewModel : Desktop.Core.PageableViewModelBase
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public ThreeD2ViewModel()
        {
            init();
        }
        #region 初始化总入口
        /// <summary>
        /// 初始化总入口
        /// </summary>
        private void init()
        {
            initMainContent();

            initCommand();
            //roadInfoList = initRoadInfo();
            //markPointList = initMarkPoint();
            //init3DModels();
            //initSocket();
            //getAgvInfoPageData();
            //ClientDataEntities = new ObservableCollection<ClientDataEntity>(getClientDataEntities(1, 100));
            //this.client.Send(GetMessage(JsonHelper.ToJson(ClientDataEntities)));



            //所有可视对象生成完成，调整视角适合查看
            OnExecuteDefaultViewPositionCommand();
        }

        /// <summary>
        /// 初始化3D视窗。增加灯光、默认摄像机等公共对象
        /// </summary>
        private void initMainContent()
        {
            //添加灯光
            mainContent.Children.Add(new HelixToolkit.Wpf.SunLight());

            //保存默认相机对象，供恢复默认视角使用
            defaultCamera = this.MainContent.Camera.Clone();
            mainContent.ZoomExtents();
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

        /// <summary>
        /// 初始化3D模型
        /// </summary>
        private void init3DModels(List<string> modelsNames)
        {
            //var modelsNames = System.IO.Directory.GetFiles(Utility.ConstValue.AppPath + @"3D_Models", "*.STL", SearchOption.TopDirectoryOnly).ToList();
            //var modelsNames = System.IO.Directory.GetFiles(Utility.ConstValue.AppPath + @"3D_Models/工件/1号工件", "*.STL", SearchOption.TopDirectoryOnly).ToList();

            HelixToolkit.Wpf.ModelImporter import = new HelixToolkit.Wpf.ModelImporter();
            //joints = new List<Joint>();
            Model3DGroup model3DGroup = new Model3DGroup();
            foreach (string modelName in modelsNames)
            {
                var materialGroup = new MaterialGroup();
                Color mainColor = Colors.White;
                EmissiveMaterial emissMat = new EmissiveMaterial(new SolidColorBrush(mainColor));
                System.Windows.Media.Media3D.DiffuseMaterial diffMat = new System.Windows.Media.Media3D.DiffuseMaterial(new SolidColorBrush(mainColor));
                SpecularMaterial specMat = new SpecularMaterial(new SolidColorBrush(mainColor), 200);
                materialGroup.Children.Add(emissMat);
                materialGroup.Children.Add(diffMat);
                materialGroup.Children.Add(specMat);

                var link = import.Load(modelName);
                System.Windows.Media.Media3D.GeometryModel3D model = link.Children[0] as System.Windows.Media.Media3D.GeometryModel3D;
                model.Material = materialGroup;
                model.BackMaterial = materialGroup;
                //joints.Add(new Joint(link));
                model3DGroup.Children.Add(link);
            }

            ModelVisual3D modelVisual3D = new ModelVisual3D();

            modelVisual3D.Content = model3DGroup;

            mainContent.Children.Add(modelVisual3D);
            mainContent.InputBindings.Add(new MouseBinding(this.PointSelectionCommand, new MouseGesture(MouseAction.LeftClick)));

        }


        #endregion

        /// <summary>
        /// 默认相机，恢复默认视角使用
        /// </summary>
        private System.Windows.Media.Media3D.ProjectionCamera defaultCamera;

        /// <summary>
        /// 选中的3D模型
        /// </summary>
        private IList<System.Windows.Media.Media3D.Model3D> selectedModels;

        #region 命令定义和初始化

        /// <summary>
        /// 打开文件命令
        /// </summary>
        public DelegateCommand OpenFileCommand { get; set; }

        /// <summary>
        /// 3D控件左击
        /// </summary>
        public HelixToolkit.Wpf.PointSelectionCommand PointSelectionCommand { get; private set; }

        /// <summary>
        /// 默认视角命令
        /// </summary>
        public DelegateCommand DefaultViewPositionCommand { get; set; }

        /// <summary>
        /// 初始化界面相关的命令
        /// </summary>
        private void initCommand()
        {

            this.PointSelectionCommand = new HelixToolkit.Wpf.PointSelectionCommand(MainContent.Viewport, OnExecuteModelsPointSelectionCommand, OnExecuteVisualsPointSelectionCommand);
            this.DefaultViewPositionCommand = new DelegateCommand(OnExecuteDefaultViewPositionCommand);
            this.OpenFileCommand = new DelegateCommand(OnExecuteOpenFileCommand);

        }

        #endregion

        #region 命令和消息等执行函数



        /// <summary>
        /// 模型选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExecuteModelsPointSelectionCommand(object sender, HelixToolkit.Wpf.ModelsSelectedEventArgs args)
        {
            this.ChangeMaterial(this.selectedModels, HelixToolkit.Wpf.Materials.LightGray);
            this.selectedModels = args.SelectedModels;
            var rectangleSelectionArgs = args as HelixToolkit.Wpf.ModelsSelectedByRectangleEventArgs;
            if (rectangleSelectionArgs != null)
            {
                this.ChangeMaterial(
                    this.selectedModels,
                    rectangleSelectionArgs.Rectangle.Size != default(Size) ? HelixToolkit.Wpf.Materials.Red : HelixToolkit.Wpf.Materials.Green);
            }
            else
            {
                this.ChangeMaterial(this.selectedModels, HelixToolkit.Wpf.Materials.Orange);
            }
        }


        /// <summary>
        /// 执行恢复默视角命令，恢复相机位置和缩放到填充模式
        /// </summary>
        private void OnExecuteDefaultViewPositionCommand()
        {
            //Task.Factory.StartNew(new Action(init));
            //OnExecuteSizeChangedCommand();
            this.MainContent.Camera = defaultCamera.Clone();
            //this.MainContent.CameraController..LookDirection = defaultCameraLookDirection;
            this.mainContent.ZoomExtents();
        }
        private void OnExecuteOpenFileCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = $"{HelixToolkit.Wpf.SharpDX.Assimp.Importer.SupportedFormatsString}";
            ;
            if (dialog.ShowDialog() == true)
            {
                init3DModels(dialog.FileNames.ToList());
            }
        }
        #endregion

        #region 内部函数

        /// <summary>
        /// Visual选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExecuteVisualsPointSelectionCommand(object sender, HelixToolkit.Wpf.VisualsSelectedEventArgs e)
        {

        }

        /// <summary>
        /// 改变材质（颜色）
        /// </summary>
        /// <param name="models"></param>
        /// <param name="material"></param>
        private void ChangeMaterial(IEnumerable<Model3D> models, System.Windows.Media.Media3D.Material material)
        {
            if (models == null)
            {
                return;
            }

            foreach (var model in models)
            {
                var geometryModel = model as System.Windows.Media.Media3D.GeometryModel3D;
                if (geometryModel != null)
                {
                    geometryModel.Material = geometryModel.BackMaterial = material;
                }
            }
        }

        #endregion



        private HelixToolkit.Wpf.HelixViewport3D mainContent = new HelixToolkit.Wpf.HelixViewport3D()
        {
            ZoomExtentsWhenLoaded = true,
            Background = Utility.Windows.ResourceHelper.FindResource("V3DBackgroundBrush") as Brush,
            ShowCoordinateSystem = true,
            //ViewCubeBackText = "北",
            //ViewCubeFrontText = "南",
            //ViewCubeLeftText = "西",
            //ViewCubeRightText = "东",
            //ViewCubeTopText = "上"
            //ViewCubeBottomText
            //CoordinateSystemLabelX = "西(X)",
            //CoordinateSystemLabelY = "南(Y)",
            //CoordinateSystemLabelZ = "上(Z)",

        };// Canvas() { Background = Utility.Windows.ResourceHelper.FindResource("WorkshopBackBrush1") as Brush };

        /// <summary>
        /// 主控件
        /// </summary>
        public HelixToolkit.Wpf.HelixViewport3D MainContent
        {
            get { return mainContent; }
            set { SetProperty(ref mainContent, value); }
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
