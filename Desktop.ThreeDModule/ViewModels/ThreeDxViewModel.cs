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
            //initCommand();
            //initMenus();

        }
        /// <summary>
        /// 默认相机，恢复默认视角使用
        /// </summary>
        private HelixToolkit.Wpf.SharpDX.Camera defaultCamera;

        private void init()
        {
            initCommand();
            initMainContent();
        }





        #region 命令定义和初始化

        /// <summary>
        /// 打开文件命令
        /// </summary>
        public DelegateCommand OpenFileCommand { get; set; }

        /// <summary>
        /// 初始化界面相关的命令
        /// </summary>
        private void initCommand()
        {
            this.OpenFileCommand = new DelegateCommand(OnExecuteOpenFileCommand);

        }

        #endregion
        private SceneNodeGroupModel3D sceneNodeGroupModel3D = new SceneNodeGroupModel3D();
        private Element3DPresenter element3DPresenter = new Element3DPresenter();
        private HelixToolkitScene scene;
        private IEffectsManager effectsManager = new DefaultEffectsManager();

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
                                sceneNodeGroupModel3D.AddNode(scene.Root);
                                //if (scene.HasAnimation)
                                //{
                                //    foreach (var ani in scene.Animations)
                                //    {
                                //        Animations.Add(ani);
                                //    }
                                //}
                                //foreach (var n in scene.Root.Traverse())
                                //{
                                //    n.Tag = new AttachedNodeViewModel(n);
                                //}
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
        }
        #endregion


        //private void OpenFile()
        //{
        //    if (isLoading)
        //    {
        //        return;
        //    }
        //    string path = OpenFileDialog(OpenFileFilter);
        //    if (path == null)
        //    {
        //        return;
        //    }
        //    StopAnimation();

        //    IsLoading = true;
        //    Task.Run(() =>
        //    {
        //        var loader = new Importer();
        //        return loader.Load(path);
        //    }).ContinueWith((result) =>
        //    {
        //        IsLoading = false;
        //        if (result.IsCompleted)
        //        {
        //            scene = result.Result;
        //            Animations.Clear();
        //            GroupModel.Clear();
        //            if (scene != null)
        //            {
        //                if (scene.Root != null)
        //                {
        //                    foreach (var node in scene.Root.Traverse())
        //                    {
        //                        if (node is MaterialGeometryNode m)
        //                        {
        //                            if (m.Material is PBRMaterialCore pbr)
        //                            {
        //                                pbr.RenderEnvironmentMap = RenderEnvironmentMap;
        //                            }
        //                            else if (m.Material is PhongMaterialCore phong)
        //                            {
        //                                phong.RenderEnvironmentMap = RenderEnvironmentMap;
        //                            }
        //                        }
        //                    }
        //                }
        //                GroupModel.AddNode(scene.Root);
        //                if (scene.HasAnimation)
        //                {
        //                    foreach (var ani in scene.Animations)
        //                    {
        //                        Animations.Add(ani);
        //                    }
        //                }
        //                foreach (var n in scene.Root.Traverse())
        //                {
        //                    n.Tag = new AttachedNodeViewModel(n);
        //                }
        //            }
        //        }
        //        else if (result.IsFaulted && result.Exception != null)
        //        {
        //            MessageBox.Show(result.Exception.Message);
        //        }
        //    }, TaskScheduler.FromCurrentSynchronizationContext());
        //}

        private bool isLoading = false;
        public bool IsLoading
        {
            private set => SetProperty(ref isLoading, value);
            get => isLoading;
        }




        /// <summary>
        /// 初始化3D视窗。增加灯光、默认摄像机等公共对象
        /// </summary>
        private void initMainContent()
        {
            //添加灯光
            //mainContent.Children.Add(new HelixToolkit.Wpf.SunLight());

            //保存默认相机对象，供恢复默认视角使用
            defaultCamera = this.MainContent.Camera.Clone() as HelixToolkit.Wpf.SharpDX.Camera;
            // mainContent.ZoomExtents();
            mainContent.ShowCoordinateSystem = true;
            mainContent.ShowFrameRate = true;
            mainContent.EffectsManager = effectsManager;

            //mainContent.EffectsManager = manager;
            mainContent.Items.Add(new DirectionalLight3D() { Direction = new System.Windows.Media.Media3D.Vector3D(-1, -1, -1) });
            mainContent.Items.Add(new AmbientLight3D() { Color = Color.FromArgb(255, 50, 50, 50) });
            mainContent.Items.Add(element3DPresenter);
            element3DPresenter.Content = sceneNodeGroupModel3D;
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
        private Viewport3DX mainContent = new Viewport3DX()
        {
            //ZoomExtentsWhenLoaded = true,
            Background = Utility.Windows.ResourceHelper.FindResource("V3DBackgroundBrush") as Brush,

            //BackgroundColor = Colors.Black,
            //ShowCoordinateSystem = true,
            //ShowFrameRate = true,

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
        public Viewport3DX MainContent
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
