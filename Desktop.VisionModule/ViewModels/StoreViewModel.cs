using Assimp;
using Desktop.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Models;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Prism.Commands;
using SkiaSharp;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Utility.Windows;
using SharpDX.Direct3D9;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Desktop.VisionModule.ViewModels
{
    /// <summary>
    /// 入库检测VM
    /// </summary>
    public class StoreViewModel : Desktop.Core.PageableViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StoreViewModel()
        {
            init();

        }


        private void init()
        {
            initCommand();
            //initMainContent();
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
        /// start命令
        /// </summary>
        public DelegateCommand StartCommand { get; set; }

        ///// <summary>
        ///// 3D控件左击
        ///// </summary>
        //public HelixToolkit.Wpf.PointSelectionCommand PointSelectionCommand { get; private set; }

        /// <summary>
        /// 捕获图像命令
        /// </summary>
        public DelegateCommand CaptureImageCommand { get; set; }

        /// <summary>
        /// 初始化界面相关的命令
        /// </summary>
        private void initCommand()
        {
            this.StartCommand = new DelegateCommand(OnExecuteStartCommand);
            this.CaptureImageCommand = new DelegateCommand(OnExecuteCaptureImageCommand);
            //this.DefaultViewPositionCommand = new DelegateCommand(OnExecuteDefaultViewPositionCommand);
            //this.PointSelectionCommand = new HelixToolkit.Wpf.PointSelectionCommand(MainContent, OnExecuteModelsPointSelectionCommand, OnExecuteVisualsPointSelectionCommand);

            //HitLineGeometry.Positions = new Vector3Collection(2);
            //HitLineGeometry.Positions.Add(SharpDX.Vector3.Zero);
            //HitLineGeometry.Positions.Add(SharpDX.Vector3.Zero);
            //HitLineGeometry.Indices = new IntCollection(2);
            //HitLineGeometry.Indices.Add(0);
            //HitLineGeometry.Indices.Add(1);
        }

        #endregion


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

        //protected OrthographicCamera defaultOrthographicCamera = new OrthographicCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 1, FarPlaneDistance = 100 };

        //protected PerspectiveCamera defaultPerspectiveCamera = new PerspectiveCamera { Position = new System.Windows.Media.Media3D.Point3D(0, 0, 5), LookDirection = new System.Windows.Media.Media3D.Vector3D(-0, -0, -5), UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 1, 0), NearPlaneDistance = 0.5, FarPlaneDistance = 150 };

        //private HelixToolkit.Wpf.SharpDX.Camera camera;
        //public HelixToolkit.Wpf.SharpDX.Camera Camera
        //{
        //    get
        //    {
        //        return camera;
        //    }

        //    protected set
        //    {
        //        SetProperty(ref camera, value);
        //        CameraModel = value is PerspectiveCamera
        //                               ? Perspective
        //                               : value is OrthographicCamera ? Orthographic : null;
        //    }
        //}
        ///// <summary>
        ///// 默认相机，恢复默认视角使用
        ///// </summary>
        //private HelixToolkit.Wpf.SharpDX.Camera defaultCamera;

        #region  界面显示的视频和图片
        //private Viewport3DX mainContent = new Viewport3DX()
        //{
        //    //Background = Utility.Windows.ResourceHelper.FindResource("V3DBackgroundBrush") as Brush
        //    Background = Utility.Windows.ResourceHelper.FindResource("ControlBackgroundBrush") as Brush
        //};

        ///// <summary>
        ///// 主控件
        ///// </summary>
        //public Viewport3DX MainContent
        //{
        //    get { return mainContent; }
        //    set { SetProperty(ref mainContent, value); }
        //}

        private BitmapSource bitmapCamera;

        /// <summary>
        /// 实时视频流
        /// </summary>
        public BitmapSource BitmapCamera
        {
            get { return bitmapCamera; }
            set { SetProperty(ref bitmapCamera, value); }
        }


        private BitmapSource captureImage;

        /// <summary>
        /// 已经捕获的图像
        /// </summary>
        public BitmapSource CaptureImage
        {
            get { return captureImage; }
            set { SetProperty(ref captureImage, value); }
        }



        private VisionResult visionResult;

        /// <summary>
        /// 结果
        /// </summary>
        public VisionResult VisionResult
        {
            get { return visionResult; }
            set { SetProperty(ref visionResult, value); }
        }


        #endregion




        #region 命令和消息等执行函数



        VideoCapture capture = new VideoCapture(0); // 0 表示使用默认摄像头

        Mat frame = new Mat();


        private void OnExecuteStartCommand()
        {
            capture.Set(VideoCaptureProperties.FrameWidth, 1920);
            capture.Set(VideoCaptureProperties.FrameHeight, 1080);


            if (!capture.IsOpened())
            {
                Console.WriteLine("Error: Camera not found!");
                return;
            }

            while (true)
            {
                capture.Read(frame);
                if (frame.Empty())
                    break;
                //Cv2.ImShow("Camera", frame);

                // 按下 ESC 键退出
                if (Cv2.WaitKey(1) == 27) break;
                //BitmapCamera = MatToBitmapImage(frame);
                //Thread.Sleep(2000);
                //BitmapCamera= OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(frame);
                //BitmapCamera= OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(frame);
                BitmapCamera = frame.ToBitmapSource();
            }

            //frame.ImWrite(@"C:\Users\Admin\source\repos\myMLApp\myMLApp\Images\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss fffffff") + ".jpg");

            //capture.Release();
            //Cv2.DestroyAllWindows();












        }



        private void OnExecuteCaptureImageCommand()
        {

            try
            {

                string imagePath = Utility.ConstValue.AppPath + @"Images\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss fffffff") + ".png";
                frame.ImWrite(imagePath);

                CaptureImage = BitmapCamera = frame.ToBitmapSource();
                string postdata = $"{{\"imagePath\":\"{imagePath}\"}}";

                HttpClient httpClient = new HttpClient();
                var r = httpClient.PostAsync($"https://localhost:7091/VehicleVision?imagePath={imagePath}", new StringContent(postdata));
                //var r = httpClient.PostAsync($"https://localhost:7091/VehicleVision", new StringContent(postdata));
                string a = r.Result.Content.ReadAsStringAsync().Result.ToString();
                this.VisionResult = Utility.JsonHelper.FromJson<VisionResult>(a);
                LogHelper.Logger.Info(a);

                // string rr = r.GetAwaiter().GetResult().Content.ToString();
                //var r = Utility.HttpClientHelper.PostJsonAsync($"https://localhost:7091/VehicleVision", postdata, null, new Dictionary<string, string>());

                //VisionResult result = Utility.JsonHelper.FromJson<VisionResult>(r.Result.ToString());



                //Task<VisionResult> result = Utility.HttpClientHelper.AsyncPostResponse<VisionResult>($"https://localhost:57911/predict", postdata);



                //if (result.Result != null)
                //{

                //    LogHelper.Logger.Info(result.Result.predictedLabel);
                //    LogHelper.Logger.Info(result.Result.score.ToString());
                //    LogHelper.Logger.Info(result.Result.label.ToString());
                //    LogHelper.Logger.Info(result.Result.imageSource.ToString());
                //    LogHelper.Logger.Info(result.Result.predictedLabel.ToString());
                //    LogHelper.Logger.Info(result.Result.score.ToString());

                //}
                //else
                //{
                //    MessageBox.Show("没有返回结果");
                //}

                //// Create single instance of sample data from first line of dataset for model input
                //var imageBytes = File.ReadAllBytes(@"C:\Users\Admin\Desktop\paint.png");
                //AudiMLModel.ModelInput sampleData = new AudiMLModel.ModelInput()
                //{
                //    //ImageSource = frame.ToBytes(),
                //    ImageSource = imageBytes,
                //};

                //// Make a single prediction on the sample data and print results.
                //var sortedScoresWithLabel = AudiMLModel.PredictAllLabels(sampleData);


                //Console.WriteLine($"{"Class",-40}{"Score",-20}");
                //Console.WriteLine($"{"-----",-40}{"-----",-20}");

                //foreach (var score in sortedScoresWithLabel)
                //{
                //    Console.WriteLine($"{score.Key,-40}{score.Value,-20}");
                //}


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        //private void SceneNodeGroupModel3D_Mouse3DDown(object sender, MouseDown3DEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //    SelectedGeometry = e.HitTestResult.Geometry;


        //}

        //private List<BoneSkinMeshNode> boneSkinNodes = new List<BoneSkinMeshNode>();
        //private List<BoneSkinMeshNode> skeletonNodes = new List<BoneSkinMeshNode>();

        //private void M_MouseDown(object sender, SceneNodeMouseDownArgs e)
        //{

        //    //e.HitResult.Geometry.Colors = new Color4Collection(50);
        //    var v = e.HitResult.Geometry;

        //    //public Material SelectedMaterial { get; } = new PhongMaterial() { EmissiveColor = Color.Yellow };

        //    var result = e.HitResult;

        //    //e.Source.Material= new PhongMaterial() { EmissiveColor = Color.Yellow };
        //    // var vv = (e.Source as MeshGeometryModel3D);
        //    //e.Source.Material = new PhongMaterial() { EmissiveColor = Color.Yellow };


        //    HitLineGeometry.Positions[0] = result.PointHit - result.NormalAtHit * 0.5f;
        //    HitLineGeometry.Positions[1] = result.PointHit + result.NormalAtHit * 0.5f;
        //    HitLineGeometry.UpdateVertices();

        //    //viewModel.SelectedGeometry = e.HitTestResult.Geometry;

        //}
        //public LineGeometry3D HitLineGeometry { get; } = new LineGeometry3D() { IsDynamic = true };

        //private IList<BatchedMeshGeometryConfig> batchedMeshes;
        //public IList<BatchedMeshGeometryConfig> BatchedMeshes
        //{
        //    set
        //    {
        //        SetProperty(ref batchedMeshes, value);
        //    }
        //    get
        //    {
        //        return batchedMeshes;
        //    }
        //}
        //public System.Windows.Media.Media3D.Transform3D BatchedTransform
        //{
        //    get;
        //} = new System.Windows.Media.Media3D.ScaleTransform3D(0.1, 0.1, 0.1);

        //private Geometry3D selectedGeometry;
        //public Geometry3D SelectedGeometry
        //{
        //    set
        //    {
        //        if (SetProperty(ref selectedGeometry, value))
        //        {
        //            SelectedTransform = new System.Windows.Media.Media3D.MatrixTransform3D(BatchedMeshes.Where(x => x.Geometry == value).Select(x => x.ModelTransform).First().ToMatrix3D() * BatchedTransform.Value);
        //        }
        //    }
        //    get { return selectedGeometry; }
        //}

        //private System.Windows.Media.Media3D.Transform3D selectedTransform;
        //public System.Windows.Media.Media3D.Transform3D SelectedTransform
        //{
        //    set
        //    {
        //        SetProperty(ref selectedTransform, value);
        //    }
        //    get { return selectedTransform; }
        //}

        ///// <summary>
        ///// 执行恢复默视角命令，恢复相机位置和缩放到填充模式
        ///// </summary>
        //private void OnExecuteDefaultViewPositionCommand()
        //{
        //    //Task.Factory.StartNew(new Action(init));
        //    //OnExecuteSizeChangedCommand();
        //    this.mainContent.ZoomExtents();
        //    this.MainContent.Camera = defaultCamera.Clone() as HelixToolkit.Wpf.SharpDX.Camera;
        //    //this.MainContent.CameraController..LookDirection = defaultCameraLookDirection;
        //}

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
            capture.Release();
            capture.Dispose();

            Cv2.DestroyAllWindows();
            LogHelper.Logger.Debug($"释放资源：{this.ToString()}");
        }

    }




}
