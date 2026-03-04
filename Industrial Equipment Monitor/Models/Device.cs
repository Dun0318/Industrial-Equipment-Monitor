using System;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace Industrial_Equipment_Monitor.Models
{
    /// <summary>
    /// 장비 정보를 나타내는 모델 클래스
    /// </summary>
    public class Device
    {
        /// <summary>
        /// 장비 ID  
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 장비 위치 
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 현재 장비의 온도
        /// </summary>
        public int Temperature { get; set; }

        /// <summary>
        /// 장비 상태 (예: 정상, 경고, 위험)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 마지막 업데이트 시간
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// 온도에 따라 상태 색상을 반환해줌
        /// 40도 이하 = 녹색, 40도 초과 70도 이하 = 주황색, 71도 이상 = 빨간색
        /// </summary>
        [JsonIgnore]
        public Brush StatusColor
        {
            get
            {
                if (Temperature > 70)
                    return Brushes.Red;
                if (Temperature > 40)
                    return Brushes.Orange;

                return Brushes.Green;
            }
        }
            
    }
}
