using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Industrial_Equipment_Monitor.Models;

namespace Industrial_Equipment_Monitor.Services
{
    /// <summary>
    /// 장비 데이터를 JSON 파일로 저장하고 불러오는 서비스 클래스 
    /// 프로그램 종료 후에도 데이터를 유지하기 위한 기능을 담당 
    /// </summary>
    public class JsonService
    {
        /// <summary>
        /// 장비 데이터가 저장될 JSON 파일의 경로
        /// 프로그램 실행 폴더에 devices.json 파일을 생성하여 데이터를 저장
        /// </summary>
        private const string filePath = "devices.json";

        /// <summary>
        /// 장비 리스트를 JSON 파일로 저장
        /// </summary>
        /// <param name="devices"> 저장할 장비 리스트 </param>

        public void Save(List<Device> devices)
        {
            // devices 객체 리스트를 JSON 문자열로 전환
            // WriteIndented = true = 사람이 읽기 쉽게 줄바꿈 포함
            // Writelndented = false = 줄바꿈 없이 한 줄로 저장
            var json = JsonSerializer.Serialize(devices, new JsonSerializerOptions { WriteIndented = true });
            
            // JSON 문자열을 파일로 저장
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// JSON 파일에서 장비 데이터를 불러오는 기능
        /// 프로그램 시작 시 기존 데이터를 복원할 때 사용
        /// </summary>
        /// <returns> 저장된 장비 리스트</returns>
        public List<Device> Load()
        {
            // JSON 파일이 존재하지 않으면 빈 리스트 반환
            if (!File.Exists(filePath))
                return new List<Device>();

            // JSON 파일 읽기
            var json = File.ReadAllText(filePath);

            // JSON 문자열을 Device 리스트 객체로 변환하여 반환
            // JSON 파싱 실패 시 Null 방지
            return JsonSerializer.Deserialize<List<Device>>(json) ?? new List<Device>();
        }
    }
}
