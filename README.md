# MoveAndSurvive, 2D 플랫포머 액션 게임
<img width="820" height="480" alt="Image" src="https://github.com/user-attachments/assets/b1d3aedc-8088-4f38-bc5f-0b6d54410830" />

> Unity 기반으로 플레이어 이동, 플랫폼 기믹, 적 AI를 구현한 2D 플랫포머 액션 게임입니다.

## 프로젝트 개요

| 항목 | 내용 |
|---|---|
| 프로젝트명 | MoveAndSurvive |
| 개발 기간 | 2026.06.05 ~ 2026.06.06 |
| 개발 인원 | 1명 |
| 개발 환경 | Unity 6.3 (6000.3.7f1), C# |
| 실행 환경 | Windows |
| IDE | Unity Editor, Visual Studio 2022 |

## 실행 방법
1. Unity 프로젝트를 엽니다.
2. Unity Editor에서 Play 버튼을 눌러 실행합니다.

## 구현 기능
① 플레이어 조작
- Input System 기반 좌우 이동 및 점프 구현
- 이동 방향에 따른 스프라이트 반전
- Idle / Move / Jump 상태 기반 애니메이션 전환
- Ground Check를 활용한 바닥 판정 및 점프 제한

##
② 점수 및 리스폰
- 별 아이템 획득 시 점수 증가 및 UI 갱신
- 낙사 시 체크포인트 위치로 리스폰
- 체크포인트 도달 시 리스폰 위치 갱신

### 
③ 플랫폼 기믹
- 좌우 / 상하로 이동하는 발판 구현
- 이동 발판 위 플레이어 동반 이동 처리
- 플레이어가 위에서 밟으면 사라지는 파괴 발판 구현

### 
④ 적 AI
- 플레이어 감지 범위 기반 돌진형 적 AI 구현
- 플레이어 추적 및 점프 반응형 적 AI 구현
- 낭떠러지 감지를 통한 적 이동 제한 처리

## 조작 방법
| 키 | 기능 |
|:---:|---|
| ← → / A D | 좌우 이동 |
| Space | 점프 |

## 플레이 영상
[플레이 영상](https://youtu.be/eZXO3sx0wOY?si=rDUHItoy-gXbs_OJ)
