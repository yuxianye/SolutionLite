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
using Desktop.VisionModule.Models;
using System.Xml.Linq;
using SharpDX;
using ControlzEx.Standard;
using SharpDX.Direct2D1.Effects;

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

        public DelegateCommand<string> OpenCVFunctionCommand { get; set; }


        /// <summary>
        /// 初始化界面相关的命令
        /// </summary>
        private void initCommand()
        {
            this.StartCommand = new DelegateCommand(OnExecuteStartCommand);
            this.CaptureImageCommand = new DelegateCommand(OnExecuteCaptureImageCommand);
            this.OpenCVFunctionCommand = new DelegateCommand<string>(OnExecuteOpenCVFunctionCommand);
            //this.DefaultViewPositionCommand = new DelegateCommand(OnExecuteDefaultViewPositionCommand);
            //this.PointSelectionCommand = new HelixToolkit.Wpf.PointSelectionCommand(MainContent, OnExecuteModelsPointSelectionCommand, OnExecuteVisualsPointSelectionCommand);

            //HitLineGeometry.Positions = new Vector3Collection(2);
            //HitLineGeometry.Positions.Add(SharpDX.Vector3.Zero);
            //HitLineGeometry.Positions.Add(SharpDX.Vector3.Zero);
            //HitLineGeometry.Indices = new IntCollection(2);
            //HitLineGeometry.Indices.Add(0);
            //HitLineGeometry.Indices.Add(1);
        }

        private void OnExecuteOpenCVFunctionCommand(string s)
        {

            //if (frame.Empty())
            //{
            //    //return;

            //}
            Mat mat = new Mat(@"C:\car\WIN_20250401_09_58_43_Pro.jpg");



            //# 灰度处理
            switch (s)
            {

                case "BGR2GRAY":
                    //processFrame2(frame);
                    var gray = new Mat();
                    Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);

                    Cv2.ImShow("BGR2GRAY", gray);

                    //processFrame2(frame);
                    var blurred = new Mat();
                    Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(5, 5), 0);

                    // 显示结果或保存图像
                    Cv2.ImShow("GaussianBlur", blurred);
                    //Cv2.DestroyAllWindows(); // 关闭所有窗口
                    // 使用Canny边缘检测
                    var edges = new Mat();
                    Cv2.Canny(blurred, edges, 50, 150);
                    Cv2.ImShow("Canny", edges);
                    // 查找轮廓
                    OpenCvSharp.Point[][] contours;
                    HierarchyIndex[] hierarchy;
                    Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                    var erode = new Mat();

                    Cv2.Erode(gray, erode , frame);
                    Cv2.ImShow("Erode", erode);

                    break;
                case "GaussianBlur":
                    //processFrame2(frame);
                    //var blurred = new Mat();
                    //Cv2.GaussianBlur(mat, blurred, new OpenCvSharp.Size(5, 5), 0);
                    //// 显示结果或保存图像
                    //Cv2.ImShow("GaussianBlur", blurred);
                    //Cv2.WaitKey(0); // 等待按键后关闭窗口
                    //Cv2.DestroyAllWindows(); // 关闭所有窗口
                    break;


                default:
                    break;
            }



            // 转换为灰度图
            //var gray = new Mat();
            //Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 应用高斯模糊
            //var blurred = new Mat();
            //Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(5, 5), 0);

            //// 使用Canny边缘检测
            //var edges = new Mat();
            //Cv2.Canny(blurred, edges, 50, 150);

            //// 查找轮廓
            //OpenCvSharp.Point[][] contours;
            //HierarchyIndex[] hierarchy;
            //Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            //// 筛选轮廓（例如，基于面积）
            //double minArea = 5000; // 最小面积阈值，可根据需要调整
            //var vehicleContours = new OpenCvSharp.Point[contours.Length][];
            //int idx = 0;
            //foreach (var contour in contours)
            //{
            //    double area = Cv2.ContourArea(contour);
            //    if (area > minArea)
            //    {
            //        vehicleContours[idx++] = contour;
            //    }
            //}
            //Array.Resize(ref vehicleContours, idx); // 调整数组大小以匹配实际车辆轮廓数量

            //// 在原图上绘制轮廓
            //foreach (var contour in vehicleContours)
            //{
            //    Cv2.DrawContours(frame, new[] { contour }, -1, new Scalar(0, 0, 255), 6); // 绘制红色轮廓
            //}

            // 显示结果或保存图像
            //Cv2.ImShow("Vehicle Contours", src);
            //Cv2.WaitKey(0); // 等待按键后关闭窗口
            //Cv2.DestroyAllWindows(); // 关闭所有窗口
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

        private System.Windows.Media.Imaging.BitmapSource bitmapCamera = new BitmapImage(new Uri("pack://application:,,,/Desktop.Resource;component/Images/Background.jpg"));

        /// <summary>
        /// 实时视频流
        /// </summary>
        public System.Windows.Media.Imaging.BitmapSource BitmapCamera
        {
            get { return bitmapCamera; }
            set { SetProperty(ref bitmapCamera, value); }
        }


        private System.Windows.Media.Imaging.BitmapSource captureImage = new BitmapImage(new Uri("pack://application:,,,/Desktop.Resource;component/Images/Background.jpg"));

        /// <summary>
        /// 已经捕获的图像
        /// </summary>
        public System.Windows.Media.Imaging.BitmapSource CaptureImage
        {
            get { return captureImage; }
            set { SetProperty(ref captureImage, value); }
        }



        private ObservableCollection<KeyValuePair<string, float>> visionResult;

        /// <summary>
        /// 结果
        /// </summary>
        public ObservableCollection<KeyValuePair<string, float>> VisionResult
        {
            get { return visionResult; }
            set { SetProperty(ref visionResult, value); }
        }


        private ObservableCollection<KeyValuePair<string, string>> openCVFunctionList = new ObservableCollection<KeyValuePair<string, string>>() {

        new KeyValuePair<string, string>("BGR2GRAY", "BGR2GRAY"),
        new KeyValuePair<string, string>("GaussianBlur", "GaussianBlur"),
        new KeyValuePair<string, string>("Key", "Vaule"),
        new KeyValuePair<string, string>("Key", "Vaule"),
        new KeyValuePair<string, string>("Key", "Vaule"),
        new KeyValuePair<string, string>("Key", "Vaule"),
        new KeyValuePair<string, string>("Key", "Vaule"),

        };

        /// <summary>
        /// 结果
        /// </summary>
        public ObservableCollection<KeyValuePair<string, string>> OpenCVFunctionList
        {
            get { return openCVFunctionList; }
            set { SetProperty(ref openCVFunctionList, value); }
        }

        #endregion




        #region 命令和消息等执行函数



        VideoCapture capture;

        Mat frame = new Mat();


        private void OnExecuteStartCommand()
        {
            // 0 表示使用默认摄像头
            //capture = new VideoCapture(0); 
            //capture = new VideoCapture(@"C:\AudiCar\VID_20250328_101730.mp4");
            capture = new VideoCapture(@"C:\car\WIN_20250401_09_58_43_Pro.jpg");

            //capture.Set(VideoCaptureProperties.FrameWidth, 1024);
            //capture.Set(VideoCaptureProperties.FrameHeight, 768);

            if (!capture.IsOpened())
            {
                Console.WriteLine("Error: Camera not found!");
                return;
            }

            while (true)
            {
                capture?.Read(frame);
                if (frame.Empty())
                    break;
                //Cv2.ImShow("Camera", frame);
                processFrame(frame);
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

        public void processFrame(Mat frame)
        {

            //CascadeClassifier carClassifier = new CascadeClassifier("path_to_haarcascade_car.xml");
            ////Mat frame = new Mat();
            //MatOfRect cars = new MatOfRect();

            ////while (capture.Read(frame))
            ////{
            //carClassifier.DetectMultiScale(frame, cars);
            //foreach (var rect in cars.ToArray())
            //{
            //    Cv2.Rectangle(frame, rect, Scalar.Red, 2); // 绘制矩形框
            //}
            //Cv2.ImShow("Video", frame);
            //int key = Cv2.WaitKey(20);
            //if (key == 27) // 按ESC退出
            //    break;
            //}
        }

        public void processFrame2(Mat frame)
        {
            //# 灰度处理
            var gray = new Mat();
            Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

            // 转换为灰度图
            //var gray = new Mat();
            //Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 应用高斯模糊
            var blurred = new Mat();
            Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(5, 5), 0);

            // 使用Canny边缘检测
            var edges = new Mat();
            Cv2.Canny(blurred, edges, 50, 150);

            // 查找轮廓
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(edges, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 筛选轮廓（例如，基于面积）
            double minArea = 5000; // 最小面积阈值，可根据需要调整
            var vehicleContours = new OpenCvSharp.Point[contours.Length][];
            int idx = 0;
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > minArea)
                {
                    vehicleContours[idx++] = contour;
                }
            }
            Array.Resize(ref vehicleContours, idx); // 调整数组大小以匹配实际车辆轮廓数量

            // 在原图上绘制轮廓
            foreach (var contour in vehicleContours)
            {
                Cv2.DrawContours(frame, new[] { contour }, -1, new Scalar(0, 0, 255), 6); // 绘制红色轮廓
            }

            // 显示结果或保存图像
            //Cv2.ImShow("Vehicle Contours", src);
            //Cv2.WaitKey(0); // 等待按键后关闭窗口
            //Cv2.DestroyAllWindows(); // 关闭所有窗口





















            //# 灰度处理
            //OutputArray gray = new Mat();
            //Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

            //OutputArray blur = new Mat();














            //Cv2.GaussianBlur(gray, blur, new OpenCvSharp.Size(5, 5), 0);

            //  blur = cv2.GaussianBlur(gray, (3, 3), 5)
            //mask = bgsubmog.apply(blur)

            //# 形态学处理
            //erode = cv2.erode(mask, kernel)
            //dilate = cv2.dilate(erode, kernel, iterations = 3)
            //close = cv2.morphologyEx(dilate, cv2.MORPH_CLOSE, kernel)

            //# 查找轮廓
            //contours, _ = cv2.findContours(close, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

            //# 绘制检测线
            //cv2.line(frame, (10, 550), (1200, 550), (0, 255, 255), 3)
            //Cv2.Line(frame, 10, 10, 2000, 200, Scalar.Red,5);

            //OpenCvSharp.Point pt1 = new OpenCvSharp.Point(100, 100);
            //OpenCvSharp.Point pt2 = new OpenCvSharp.Point(300, 300);

            //Cv2.Rectangle(frame, pt1, pt2, Scalar.Red, 5, LineTypes.AntiAlias, 0);//绘制矩形 参数1:操作图像 2:矩形左上点 3:矩形右下点 4:颜色 5:线宽  6:线型  7:缩放参数（0为不缩放）

            //for c in contours:
            //x, y, w, h = cv2.boundingRect(c)
            //if w >= 90 and h >= 90:
            //cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 0, 255), 2)
            //centre_p = (x + int(w / 2), y + int(h / 2))
            //cars.append(centre_p)
            //cv2.circle(frame, centre_p, 5, (0, 0, 255), -1)

            //for x, y in cars:
            //if 593 < y < 607:
            //car_n += 1
            //cars.remove((x, y))

            //cv2.putText(frame, "Cars Count: " + str(car_n), (500, 60), cv2.FONT_HERSHEY_SIMPLEX, 2, (0, 0, 255), 5)
            //cv2.imshow('video', frame)


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


                var vv = Utility.JsonHelper.FromJson<ObservableCollection<KeyValuePair<string, float>>>(a);


                this.VisionResult = vv;
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
