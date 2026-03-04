# Industrial Equipment Monitor

WPF 기반 산업 장비 모니터링 데스크톱 애플리케이션입니다.  
장비의 상태(Start / Stop)를 제어하고 온도를 실시간으로 모니터링할 수 있는 시스템을 구현했습니다.

본 프로젝트는 **장비 상태 관리**, **실시간 데이터 갱신**, **과열 감지 시스템**을 중심으로 개발되었습니다.

---

# 데모 (Demo)

## 장비 등록 및 장비 제어

![장비 등록 및 제어](images/input_start_stop.gif)

장비를 등록하고 **Start / Stop 버튼**을 통해 장비 상태를 제어할 수 있습니다.

Running 상태에서는 **DispatcherTimer 기반 온도 시뮬레이션**이 동작하여  
온도가 실시간으로 변화합니다.

---

## 장비 수정 및 삭제

![장비 수정 및 삭제](images/delete_Update.gif)

등록된 장비의 정보를 수정하거나 삭제할 수 있습니다.

---

## 과열 경고 시스템

![과열 경고](images/과열경고문.png)

장비 온도가 **85°C 이상**이 되면

- 과열 경고 메시지 출력
- 장비 자동 정지

시스템 안전을 위한 **과열 보호 로직**을 구현했습니다.

---

# 주요 기능

### 장비 관리

- 장비 등록 (Device ID / Location)
- 장비 삭제
- 장비 정보 수정

### 장비 제어

- 장비 작동 시작 (Start)
- 장비 작동 중지 (Stop)

### 실시간 온도 모니터링

- DispatcherTimer 기반 실시간 데이터 갱신
- Running 상태 장비만 온도 변화
- 랜덤 온도 시뮬레이션

### 과열 감지 시스템

- 85°C 이상 감지 시 경고 메시지 출력
- 장비 자동 Stop 처리

### 데이터 저장

- JSON 기반 장비 데이터 저장
- 프로그램 재실행 시 데이터 복구

---

# 기술 스택

- **C#**
- **WPF**
- **.NET**
- **ObservableCollection**
- **DispatcherTimer**
- **JSON Serialization**

---

# 시스템 구조

```
UI (WPF)
 │
 ├ MainWindow
 │   ├ 장비 등록 / 수정 / 삭제
 │   ├ 장비 상태 제어
 │   ├ 온도 시뮬레이션
 │   └ 데이터 바인딩
 │
 ├ Models
 │   └ Device
 │
 └ Services
     └ JsonService
```

---

# 핵심 구현 로직

## 실시간 온도 시뮬레이션

DispatcherTimer를 이용하여  
3초마다 장비 상태를 갱신합니다.

```csharp
timer.Interval = TimeSpan.FromSeconds(3);
timer.Tick += Timer_Tick;
```

Running 상태 장비의 온도는 랜덤 범위에서 변화합니다.

```csharp
int change = random.Next(-2, 3);
device.Temperature += change;
```

---

## 과열 감지 로직

장비 온도가 **85°C 이상**이 되면  
자동으로 장비를 정지시키는 보호 로직입니다.

```csharp
if (device.Temperature >= 85)
{
    device.Status = "Stop";
}
```

---

# 프로젝트 구조

```
Industrial_Equipment_Monitor
 │
 ├ Models
 │   └ Device.cs
 │
 ├ Services
 │   └ JsonService.cs
 │
 ├ MainWindow.xaml
 ├ MainWindow.xaml.cs
 │
 └ Industrial_Equipment_Monitor.sln
```

---

# 실행 방법

1. Visual Studio 실행  
2. `Industrial_Equipment_Monitor.sln` 파일 열기  
3. 프로젝트 실행 (F5)

---

# 프로젝트를 통해 배운 점

- WPF 기반 데스크톱 애플리케이션 구조 이해
- DispatcherTimer를 활용한 실시간 데이터 갱신
- ObservableCollection을 활용한 UI 데이터 바인딩
- JSON 기반 데이터 저장 및 복원
- 상태 기반 장비 제어 로직 구현

---

# 향후 개선 방향

- 실제 센서 데이터 연동
- 데이터베이스 저장 (SQLite / MySQL)
- 장비 상태 로그 기록
- 대시보드 UI 개선
