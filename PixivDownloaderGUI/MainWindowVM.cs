using PixivDownloader2;
using PixivDownloader2.Enum;
using PixivDownloader2.Page;
using PixivDownloaderGUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PixivDownloaderGUI
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private object _collectionLock = new object();
        private ObservableCollection<IllustVM> illustCollection;
        private Dispatcher dispatcher;
        private string alertMessage;

        private int rankPageType = 0;
        private string rankDate;
        private RankPage page;

        public ObservableCollection<IllustVM> IllustCollection
        {
            get
            {
                if (illustCollection == null)
                {
                    illustCollection = new ObservableCollection<IllustVM>();
                    BindingOperations.EnableCollectionSynchronization(illustCollection, _collectionLock);
                }
                return illustCollection;
            }
            set
            {
                illustCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IllustCollection)));
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string AlertMessage
        {
            get => alertMessage;
            set
            {
                alertMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlertMessage)));
            }
        }

        /// <summary>
        /// 排行榜类型
        /// </summary>
        public int RankPageType
        {
            get => rankPageType;
            set
            {
                if (rankPageType != value)
                {
                    rankPageType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RankPageType)));
                    //加载排行榜
                    Task.Run(() =>LoadRankPage());
                }
            }
        }

        public bool HasLogined { get => LoginPage.LoginSuccess; }

        /// <summary>
        /// 是否有前一天的排行榜
        /// </summary>
        public bool HasPrevRank
        {
            get
            {
                if (page != null && page.HasPrevRank)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 是否有后一天的排行榜
        /// </summary>
        public bool HasNextRank
        {
            get
            {
                if (page != null && page.HasNextRank)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 当前排行榜的日期
        /// </summary>
        public string RankDate
        {
            get => rankDate;
            set
            {
                if (rankDate != value)
                {
                    rankDate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RankDate)));
                }
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand PrevDayCommand { get; set; }
        public ICommand NextDayCommand { get; set; }
        public ICommand LoadRankCommand { get; set; }

        public MainWindowVM(Dispatcher dispatcher)
        {
            LoginCommand = new DelegateCommand<object>(LoginCommandHanlder);
            PrevDayCommand = new DelegateCommand<object>(PrevDayCommandHandler);
            NextDayCommand = new DelegateCommand<object>(NextDayCommandHandler);
            LoadRankCommand = new DelegateCommand<object>(LoadRankCommandHandler);
            this.dispatcher = dispatcher;

            ShowLoginWindowAndLogin();
        }

        /// <summary>
        /// 用户点击了界面上的登录按钮
        /// </summary>
        /// <param name="obj"></param>
        private void LoginCommandHanlder(object obj)
        {
            ShowLoginWindowAndLogin();
        }

        /// <summary>
        /// 显示登陆框并等待用户登陆
        /// </summary>
        private void ShowLoginWindowAndLogin()
        {
            //LoginWindow login = new LoginWindow();
            //WebLoginWindow login = new WebLoginWindow();
            CookieLoginWindow login = new CookieLoginWindow();
            if (login.ShowDialog() == true)
            {
                DispatcherShowMessage("登录中...", false);
                Task.Run(() =>
                {
                    try
                    {
                        //异步登陆
                        //if (LoginPixiv(login.UserName, login.Password))
                        if (LoginPixiv(login.CookieStr))
                        {
                            DispatcherShowMessage("登录成功");
                        }
                        else
                        {
                            DispatcherShowMessage("登录Pixiv失败，原因：用户名密码错误");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        DispatcherShowMessage("登录Pixiv失败，原因：" + ex.Message);
                        return;
                    }
                    //更新界面登陆状态
                    UpdateLoginStatus();
                    //异步加载排行榜
                    LoadRankPage();
                });
            }
        }

        /// <summary>
        /// 调用登陆接口登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登陆是否成功</returns>
        private bool LoginPixiv(string userName, string password)
        {
            LoginPage.Init(userName, password);
            return LoginPage.LoginSuccess;
        }

        private bool LoginPixiv(string cookieStr)
        {
            LoginPage.Init(cookieStr);
            return LoginPage.LoginSuccess;
        }

        /// <summary>
        /// 用户点击了后一天按钮
        /// </summary>
        /// <param name="obj"></param>
        private void NextDayCommandHandler(object obj)
        {
            if (page != null && page.HasNextRank)
            {
                RankDate = page.NextRankDate;
                LoadRankPageAsync();
            }
        }

        /// <summary>
        /// 用户点击了前一天按钮
        /// </summary>
        /// <param name="obj"></param>
        private void PrevDayCommandHandler(object obj)
        {
            if (page != null && page.HasPrevRank)
            {
                RankDate = page.PrevRankDate;
                LoadRankPageAsync();
            }
        }

        /// <summary>
        /// 用户在日期文本框中按下了回车
        /// </summary>
        /// <param name="obj"></param>
        private void LoadRankCommandHandler(object obj)
        {
            if (page != null)
            {
                LoadRankPageAsync();
            }
        }

        private void LoadRankPageAsync()
        {
            Task.Run(() => LoadRankPage());
        }

        /// <summary>
        /// 加载排行榜
        /// </summary>
        private void LoadRankPage()
        {
            //dispatcher?.Invoke(() => IllustCollection.Clear());
            lock (_collectionLock)
                IllustCollection.Clear();
            
            page = new RankPage();
            page.Init((RankType)RankPageType, 1, RankDate, false);
            //更新排行榜日期及其他信息
            UpateRankDate();
            foreach (var content in page.IllustCollection)
            {
                lock (_collectionLock)
                    IllustCollection.Add(new IllustVM(dispatcher, content, page.ReferUrl));
            }
        }

        public void LazyLoadAsync()
        {
            Task.Run(() =>
            {
                LazyLoad();
            });
        }

        /// <summary>
        /// 加载此排行榜的剩余数据
        /// </summary>
        private void LazyLoad()
        {
            if (page != null)
            {
                if (page.CanLazyLoad())
                {
                    DispatcherShowMessage("加载中...", false);
                    var picList = page.LazyLoad();
                    lock (_collectionLock)
                        picList?.ForEach(p => IllustCollection.Add(new IllustVM(dispatcher, p, page.ReferUrl)));
                    ShowMessage(null);
                }
            }
        }

        /// <summary>
        /// 更新排行榜日期及其他信息
        /// </summary>
        private void UpateRankDate()
        {
            if (page != null)
            {
                RankDate = page.RankDate;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasNextRank)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasPrevRank)));
            }
        }

        /// <summary>
        /// 更新界面上的登陆状态
        /// </summary>
        private void UpdateLoginStatus()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasLogined)));
        }

        /// <summary>
        /// 在主线程外显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void DispatcherShowMessage(string msg, bool autoDismiss = true)
        {
            dispatcher?.Invoke(async () =>
            {
                await ShowMessage(msg, autoDismiss);
            });
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private async Task ShowMessage(string msg, bool autoDismiss = true)
        {
            AlertMessage = msg;
            if (autoDismiss)
            {
                await Task.Delay(5000);
                AlertMessage = null;
            }
        }

        /// <summary>
        /// 主界面退出时触发
        /// 清理缓存
        /// </summary>
        public void CleanUp()
        {
            FileManager.CleanCache();
            FileManager.CleanMangaPreviewCache();
        }
    }
}
