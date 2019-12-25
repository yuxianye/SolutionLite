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
using Utility.Windows;

namespace Desktop.ThreeDModule.ViewModels
{
    /// <summary>
    /// 用户管理VM
    /// </summary>
    public class ThreeDx2ViewModel : Desktop.Core.PageableViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        //public ThreeDViewModel()
        //{
        //    //initCommand();
        //    //initMenus();

        //    //EventAggregator.GetEvent<UpListDataEvent>().Subscribe(upListData, Prism.Events.ThreadOption.BackgroundThread, false, x =>
        //    //{
        //    //    return (x is User);

        //    //});
        //    //getPageData(1, PageSize);
        //}


        private string OpenFileFilter = $"{HelixToolkit.Wpf.SharpDX.Assimp.Importer.SupportedFormatsString}";
        private string ExportFileFilter = $"{HelixToolkit.Wpf.SharpDX.Assimp.Exporter.SupportedFormatsString}";
        private bool showWireframe = false;
        public bool ShowWireframe
        {
            set
            {
                if (SetProperty(ref showWireframe, value))
                {
                    ShowWireframeFunct(value);
                }


            }
            get
            {
                return showWireframe;
            }
        }

        private bool renderFlat = false;
        public bool RenderFlat
        {
            set
            {
                if (SetProperty(ref renderFlat, value))
                {
                    RenderFlatFunct(value);
                }
            }
            get
            {
                return renderFlat;
            }
        }

        private bool renderEnvironmentMap = true;
        public bool RenderEnvironmentMap
        {
            set
            {
                if (SetProperty(ref renderEnvironmentMap, value) && scene != null && scene.Root != null)
                {
                    foreach (var node in scene.Root.Traverse())
                    {
                        if (node is MaterialGeometryNode m && m.Material is PBRMaterialCore material)
                        {
                            material.RenderEnvironmentMap = value;
                        }
                    }
                }
            }
            get => renderEnvironmentMap;
        }

        public ICommand OpenFileCommand
        {
            get; set;
        }

        public ICommand ResetCameraCommand
        {
            set; get;
        }

        public ICommand DefaultVIewCommand
        {
            set; get;
        }

        public ICommand ExportCommand { private set; get; }

        private bool isLoading = false;
        public bool IsLoading
        {
            private set => SetProperty(ref isLoading, value);
            get => isLoading;
        }

        private bool enableAnimation = false;
        public bool EnableAnimation
        {
            set
            {
                if (SetProperty(ref enableAnimation, value))
                {
                    if (value)
                    {
                        StartAnimation();
                    }
                    else
                    {
                        StopAnimation();
                    }
                }
            }
            get { return enableAnimation; }
        }

        public ObservableCollection<HelixToolkit.Wpf.SharpDX.Animations.Animation> Animations { get; } = new ObservableCollection<HelixToolkit.Wpf.SharpDX.Animations.Animation>();

        public SceneNodeGroupModel3D GroupModel { get; } = new SceneNodeGroupModel3D();

        private HelixToolkit.Wpf.SharpDX.Animations.Animation selectedAnimation = null;
        public HelixToolkit.Wpf.SharpDX.Animations.Animation SelectedAnimation
        {
            set
            {
                if (SetProperty(ref selectedAnimation, value))
                {
                    StopAnimation();
                    if (value != null)
                    {
                        animationUpdater = new NodeAnimationUpdater(value);
                    }
                    else
                    {
                        animationUpdater = null;
                    }
                    if (enableAnimation)
                    {
                        StartAnimation();
                    }
                }
            }
            get
            {
                return selectedAnimation;
            }
        }

        public TextureModel EnvironmentMap { get; }

        private SynchronizationContext context = SynchronizationContext.Current;
        private HelixToolkitScene scene;
        private NodeAnimationUpdater animationUpdater;
        private List<BoneSkinMeshNode> boneSkinNodes = new List<BoneSkinMeshNode>();
        private List<BoneSkinMeshNode> skeletonNodes = new List<BoneSkinMeshNode>();
        private CompositionTargetEx compositeHelper = new CompositionTargetEx();

        private IEffectsManager effectsManager;
        public IEffectsManager EffectsManager
        {
            get { return effectsManager; }
            protected set
            {
                SetProperty(ref effectsManager, value);
            }
        }
        private string cameraModel;

        private HelixToolkit.Wpf.SharpDX.Camera camera;
        public string CameraModel
        {
            get
            {
                return cameraModel;
            }
            set
            {
                if (SetProperty(ref cameraModel, value, "CameraModel"))
                {
                    OnCameraModelChanged();
                }
            }
        }
        public event EventHandler CameraModelChanged;

        protected virtual void OnCameraModelChanged()
        {
            var eh = CameraModelChanged;
            if (eh != null)
            {
                eh(this, new EventArgs());
            }
        }
        public HelixToolkit.Wpf.SharpDX.Camera Camera
        {
            get
            {
                return camera;
            }

            protected set
            {
                SetProperty(ref camera, value, "Camera");
                CameraModel = value is PerspectiveCamera
                                       ? Perspective
                                       : value is OrthographicCamera ? Orthographic : null;
            }
        }
        protected OrthographicCamera defaultOrthographicCamera = new OrthographicCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 1, FarPlaneDistance = 100 };

        protected PerspectiveCamera defaultPerspectiveCamera = new PerspectiveCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 0.5, FarPlaneDistance = 150 };
        public const string Orthographic = "Orthographic Camera";

        public const string Perspective = "Perspective Camera";

        /// <summary>
        /// 构造函数
        /// </summary>
        public ThreeDx2ViewModel()
        {
            // on camera changed callback
            CameraModelChanged += (s, e) =>
            {
                if (cameraModel == Orthographic)
                {
                    if (!(Camera is OrthographicCamera))
                        Camera = defaultOrthographicCamera;
                }
                else if (cameraModel == Perspective)
                {
                    if (!(Camera is PerspectiveCamera))
                        Camera = defaultPerspectiveCamera;
                }
                else
                {
                    throw new HelixToolkitException("Camera Model Error.");
                }
            };

            this.OpenFileCommand = new DelegateCommand(this.OpenFile);
            EffectsManager = new DefaultEffectsManager();
            Camera = new OrthographicCamera()
            {
                LookDirection = new System.Windows.Media.Media3D.Vector3D(0, -10, -10),
                Position = new System.Windows.Media.Media3D.Point3D(0, 10, 10),
                UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0),
                FarPlaneDistance = 5000,
                NearPlaneDistance = 0.1f
            };
            ResetCameraCommand = new DelegateCommand(() =>
            {
                (Camera as OrthographicCamera).Reset();
                (Camera as OrthographicCamera).FarPlaneDistance = 5000;
                (Camera as OrthographicCamera).NearPlaneDistance = 0.1f;
            });
            ExportCommand = new DelegateCommand(() => { ExportFile(); });
            //EnvironmentMap = LoadFileToMemory("Cubemap_Grandcanyon.dds");

            ResetCameraCommand = new DelegateCommand(() =>
            {

            });
        }

        public static MemoryStream LoadFileToMemory(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open))
            {
                var memory = new MemoryStream();
                file.CopyTo(memory);
                return memory;
            }
        }
        private void OpenFile()
        {
            if (isLoading)
            {
                return;
            }
            string path = OpenFileDialog(OpenFileFilter);
            if (path == null)
            {
                return;
            }
            StopAnimation();

            IsLoading = true;
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
                    Animations.Clear();
                    GroupModel.Clear();
                    if (scene != null)
                    {
                        if (scene.Root != null)
                        {
                            foreach (var node in scene.Root.Traverse())
                            {
                                if (node is MaterialGeometryNode m)
                                {
                                    if (m.Material is PBRMaterialCore pbr)
                                    {
                                        pbr.RenderEnvironmentMap = RenderEnvironmentMap;
                                    }
                                    else if (m.Material is PhongMaterialCore phong)
                                    {
                                        phong.RenderEnvironmentMap = RenderEnvironmentMap;
                                    }
                                }
                            }
                        }
                        GroupModel.AddNode(scene.Root);
                        if (scene.HasAnimation)
                        {
                            foreach (var ani in scene.Animations)
                            {
                                Animations.Add(ani);
                            }
                        }
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
        }

        public void StartAnimation()
        {
            compositeHelper.Rendering += CompositeHelper_Rendering;
        }

        public void StopAnimation()
        {
            compositeHelper.Rendering -= CompositeHelper_Rendering;
        }

        private void CompositeHelper_Rendering(object sender, System.Windows.Media.RenderingEventArgs e)
        {
            if (animationUpdater != null)
            {
                animationUpdater.Update(Stopwatch.GetTimestamp(), Stopwatch.Frequency);
            }
        }

        private void ExportFile()
        {
            var index = SaveFileDialog(ExportFileFilter, out var path);
            if (!string.IsNullOrEmpty(path) && index >= 0)
            {
                var id = HelixToolkit.Wpf.SharpDX.Assimp.Exporter.SupportedFormats[index].FormatId;
                var exporter = new HelixToolkit.Wpf.SharpDX.Assimp.Exporter();
                exporter.ExportToFile(path, scene, id);
                return;
            }
        }


        private string OpenFileDialog(string filter)
        {
            var d = new OpenFileDialog();
            d.CustomPlaces.Clear();

            d.Filter = filter;

            if (!d.ShowDialog().Value)
            {
                return null;
            }

            return d.FileName;
        }

        private int SaveFileDialog(string filter, out string path)
        {
            var d = new SaveFileDialog();
            d.Filter = filter;
            if (d.ShowDialog() == true)
            {
                path = d.FileName;
                return d.FilterIndex - 1;//This is tarting from 1. So must minus 1
            }
            else
            {
                path = "";
                return -1;
            }
        }

        private void ShowWireframeFunct(bool show)
        {
            foreach (var node in GroupModel.GroupNode.Items.PreorderDFT((node) =>
            {
                return node.IsRenderable;
            }))
            {
                if (node is MeshNode m)
                {
                    m.RenderWireframe = show;
                }
            }
        }

        private void RenderFlatFunct(bool show)
        {
            foreach (var node in GroupModel.GroupNode.Items.PreorderDFT((node) =>
            {
                return node.IsRenderable;
            }))
            {
                if (node is MeshNode m)
                {
                    if (m.Material is PhongMaterialCore phong)
                    {
                        phong.EnableFlatShading = show;
                    }
                    else if (m.Material is PBRMaterialCore pbr)
                    {
                        pbr.EnableFlatShading = show;
                    }
                }
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


    /// <summary>
    /// Provide your own view model to manipulate the scene nodes
    /// </summary>
    /// <seealso cref="DemoCore.ObservableObject" />
    public class AttachedNodeViewModel : ViewModelBase
    {
        private bool selected = false;
        public bool Selected
        {
            set
            {
                if (SetProperty(ref selected, value))
                {
                    if (node is MeshNode m)
                    {
                        m.PostEffects = value ? $"highlight[color:#FFFF00]" : "";
                        foreach (var n in node.TraverseUp())
                        {
                            if (n.Tag is AttachedNodeViewModel vm)
                            {
                                vm.Expanded = true;
                            }
                        }
                    }
                }
            }
            get => selected;
        }

        private bool expanded = false;
        public bool Expanded
        {
            set => SetProperty(ref expanded, value);
            get => expanded;
        }

        public bool IsAnimationNode { get => node.IsAnimationNode; }

        public string Name { get => node.Name; }

        private SceneNode node;

        public AttachedNodeViewModel(SceneNode node)
        {
            this.node = node;
            node.Tag = this;
        }
    }


}
