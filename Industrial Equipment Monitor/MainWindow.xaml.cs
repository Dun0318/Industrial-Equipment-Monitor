using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Industrial_Equipment_Monitor.Models;
using Industrial_Equipment_Monitor.Services;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.VisualBasic;

namespace Industrial_Equipment_Monitor
{
    /// <summary>
    /// 장비 등록 , 상태제어 , JSON 저장 기능 담당하는 메인 윈도우 클래스
    /// </summary>
    public partial class MainWindow : Window
    {


        // 장비 목록을 저장하는 컬렉션
        ObservableCollection<Device> devices = new ObservableCollection<Device>();

        // 현재 선택된 장비
        Device selectedDevice;

        // JSON 파일 저장 , 로드 서비스
        JsonService jsonService = new JsonService();

        // 온도 시뮬레이션용 랜덤 객체
        Random random = new Random();

        // 장비 온도 주기적으로 갱신 하기 위한 타이머
        DispatcherTimer timer = new DispatcherTimer();

        



        /// <summary>
        /// 프로그램 시작 시 실행되는 생성자
        /// JSON 파일에서 기존 장비 데이터를 불러온다
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            var loadedDevices = jsonService.Load();

            foreach (var device in loadedDevices)
            {
                devices.Add(device);
            }
            // DataGrid에 장비 목록 연결
            dataGridDevices.ItemsSource = devices;

            // 3초마다 Timer_Tick 실행
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        /// <summary>
        /// 장비 선택 시 실행
        /// 선택된 장비를 selectedDevice에 저장
        /// </summary>
        private void dataGridDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDevice = dataGridDevices.SelectedItem as Device;
        }


        /// <summary>
        /// 장비 추가 버튼 클릭 시 실행
        /// 새로운 장비를 생성하여 목록에 추가하고 JSON 파일에 저장
        /// </summary>
        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            string id = txtDeviceId.Text;
            string location = txtLocation.Text;

            // 입력값 검증
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("장비 ID와 위치를 입력해주세요", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 장비 객체 생성
            Device device = new Device
            {
                DeviceId = id,
                Location = location,
                Temperature = 30,
                Status = "Stop",
                LastUpdate = DateTime.Now
            };

            // 장비 목록에 추가
            devices.Add(device);

            // JSON 파일에 저장
            try
            {
                jsonService.Save(new List<Device>(devices));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "저장 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // 입력창 초기화
            txtDeviceId.Text = "";
            txtLocation.Text = "";

            // 장비 추가 완료 메세지
            MessageBox.Show("장비가 추가되었습니다", "장비 추가 완료", MessageBoxButton.OK, MessageBoxImage.Information);  

        }


        /// <summary>
        /// Start 버튼 클릭 시 실행
        /// 선택된 장비 상태를 Running으로 변경하고 온도 시뮬레이션 시작
        /// </summary>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // 장비가 선택되지 않은 경우 경고 메세지 출력
            if (selectedDevice == null)
            {
                MessageBox.Show("장비를 선택해주세요", "선택 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;

            }
            // 장비 상태를 Rinning으로 변경
            selectedDevice.Status = "Running";

            // 장비 온도를 랜덤 값으로 생성
            selectedDevice.Temperature = random.Next(45, 80);

            // DataGrid UI 갱신
            dataGridDevices.Items.Refresh();

            // 변경된 데이터를 JSON 파일에 저장
            jsonService.Save(new List<Device>(devices));

            // 작동 시작 메세지 
            MessageBox.Show("장비가 작동을 시작했습니다", "작동 시작", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        /// <summary>
        /// Stop 버튼 클릭 시 실행
        /// 선택된 장비를 정지 상태로 변경
        /// </summary>        
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            // 장비가 선택되지 않은 경우 경고 메세지 출력
            if (selectedDevice == null)
            {
                MessageBox.Show("장비를 선택해주세요", "선택 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;

            }

            // 장비 상태를 Stop으로 변경
            selectedDevice.Status = "Stop";

            // DataGrid UI 갱신
            dataGridDevices.Items.Refresh();

            // 변경된 데이터를 JSON 파일에 저장
            jsonService.Save(new List<Device>(devices));

            // 작동 중지 메세지
            MessageBox.Show("장비가 작동을 중지했습니다", "작동 중지", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        /// <summary>
        /// Delete 버튼 클릭 시 실행
        /// 선택된 장비를 목록에서 삭제
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // 장비가 선택되지 않은 경우 경고 메세지 출력
            if (selectedDevice == null)
            {
                MessageBox.Show("삭제할 장비를 선택해주세요", "선택 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // 삭제 확인 메세지 
            var result = MessageBox.Show($"장비 {selectedDevice.DeviceId} 를 삭제하시겠습니까?", 
                "장비 삭제 확인",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // 이용자가 NO 누르면 삭제 취소
            if (result != MessageBoxResult.Yes)
            {
                return;
            }


            // 선택된 장비를 목록에서 제거
            devices.Remove(selectedDevice);

            // 선택 장비 초기화
            selectedDevice = null;

            // 변경된 데이터를 JSON 파일에 저장
            jsonService.Save(new List<Device>(devices));

            // 장비 삭제 메세지
            MessageBox.Show("장비가 삭제되었습니다", "장비 삭제 완료", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        /// <summary>
        /// Update 버튼 클릭 시 실행
        /// 선택된 장비 정보를 수정
        /// </summary>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // 수정할 장비가 선택되지 않은 경우 메세지 출력
            if (selectedDevice == null)
            {
                MessageBox.Show("수정할 장비를 선택해주세요", "선택 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // 새로운 장비 ID 입력 팝업창 띄우기
            string id = Interaction.InputBox("새로운 장비 ID를 입력하세요", "장비 ID 수정", selectedDevice.DeviceId);

            // 새로운 장비 위치 입력 팝업창 띄우기
            string location = Interaction.InputBox("새로운 장비 위치를 입력하세요", "장비 위치 수정", selectedDevice.Location);

            // 입력 값 검증
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(location))
             {
                MessageBox.Show("장비 ID와 위치를 입력해주세요", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 장비 정보 수정
            selectedDevice.DeviceId=id;
            selectedDevice.Location=location;
            selectedDevice.LastUpdate = DateTime.Now;

            // UI 갱신
            dataGridDevices.Items.Refresh();

            // JSON 저장
            jsonService.Save(new List<Device>(devices));

            MessageBox.Show("장비 정보가 수정되었습니다", "수정 완료", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        /// <summary>
        /// 타이머 이벤트 (3초마다 실행)
        /// Running 상태 장비들의 온도를 랜덤하게 변화시킴
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            
            foreach (var device in devices)
            {
                // 장비가 작동 중일 때만 온도 변화
                if (device.Status == "Running")
                {
                    // 온도를 -2 ~ +2 범위에서 랜덤하게 변화
                    int change = random.Next(-2, 3);
                    device.Temperature += change;

                    // 최소 온도 제한
                    if (device.Temperature < 20)
                        device.Temperature = 20;

                    // 마지막 업데이트 시간 기록
                    device.LastUpdate = DateTime.Now;

                    // 위험 온도 감지
                    // 85도 이상이면 장비를 정지시키고 경고 메시지 출력
                    if (device.Temperature >= 85)
                    {
                        device.Status = "Stop";

                        MessageBox.Show($"장비 {device.DeviceId} 에 온도가 위험 수준입니다! \n현재 온도 : {device.Temperature}°C \n작동을 중지합니다.",
                            "과열경고",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                    }

                }

            }

            int selectedIndex = dataGridDevices.SelectedIndex;

            // DataGrid UI 갱신
            dataGridDevices.Items.Refresh();
                

            if(selectedIndex >=0 && selectedIndex < dataGridDevices.Items.Count)
            { 
                dataGridDevices.SelectedIndex = selectedIndex;
               
            }


            // 변경된 데이터를 JSON 파일에 저장
            jsonService.Save(new List<Device>(devices));
        }

       
    }
}