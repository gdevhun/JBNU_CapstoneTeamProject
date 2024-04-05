using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 타워 타입 -> 키로 사용
public enum TowerType
{
    ArcherTower1, ArcherTower2, ArcherTower3, ArcherTower4,
    MagicTower1, MagicTower2, MagicTower3, MagicTower4
}

// 각 타워 타입에 해당하는 타워 Lv1 ~ Lv3 리스트
[System.Serializable]
public class TowerLvList
{
    public List<GameObject> towerPrefabs;
}

// 타워 설치, 해제, 업그레이드 관리
public class TowerManager : MonoBehaviour
{
    // 기타 필드
    public GameObject towerBuildPos; // 각 타워를 설치할 위치
    public static Transform selectedTowerBuildPos; // 선택된 타워를 설치할 위치
    public static GameObject selectedTowerPref; // 선택된 설치할 타워 프리팹
    public static bool isPanel = false; // 패널이 활성화중인지 체크
    public GameObject towerBuildPanel; // 타워 설치 패널
    public GameObject towerUpgradePanel; // 타워 업글 패널
    public LayerMask towerBuildPosLayerMask; // 타워 설치 위치만 클릭
    public Image upgradePanelTowerImage; // 업그레이드 패널 타워 이미지
    public TMP_Text upgradePanelTowerLvText; // 업그레이드 패널 타워 레벨 텍스트
    public TMP_Text upgradePanelTowerPriceText; // 업그레이드 패널 타워 업그레이드 비용 텍스트

    // 타워 맵핑 관련
    [SerializeField]
    public List<TowerLvList> towerPrefs = new List<TowerLvList>(); // 타워 프리팹들 -> 인스펙터에서 할당
    public List<Sprite> towerSpritePrefs = new List<Sprite>(); // 타워 이미지 바꿀 스프라이트들 -> 인스펙터에서 할당

    private Dictionary<TowerType, TowerLvList> towers = new Dictionary<TowerType, TowerLvList>(); // (타워타입, 타입에 해당하는 타워 Lv1 ~ Lv3) 맵핑
    private Dictionary<TowerType, Sprite> towerSprites = new Dictionary<TowerType, Sprite>(); // (타워타입, 타입에 해당하는 타워 스프라이트) 맵핑

    // 타워 맵핑
    void Awake()
    {
        for(int i = 0; i < towerPrefs.Count; i++)
        {
            towers[(TowerType)i] = towerPrefs[i];
            towerSprites[(TowerType)i] = towerSpritePrefs[i];
        }
    }

    // 설치 패널 활성화
    void Update()
    {
        // 패널이 활성화중이면 리턴
        if(isPanel) return;

        // 마우스 왼쪽 클릭시
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 위치 가져옴
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;

            // 히트된 설치할 위치 콜라이더 가져와서 선택된 타워 위치로 설정
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Mathf.Infinity, towerBuildPosLayerMask);

            // 히트된 콜라이더가 있는지 체크
            if (hit.collider != null)
            {
                // 타워매니저 
                TowerManager towerManager = hit.collider.gameObject.GetComponent<TowerManager>();

                // 타워매니저가 있는지 체크 
                if (towerManager != null)
                {
                    // 히트된 콜라이더의 게임오브젝트가 클릭한 영역의 설치할 위치면
                    if (hit.collider.gameObject.Equals(towerManager.towerBuildPos))
                    {
                        // 선택된 타워 설치 위치
                        selectedTowerBuildPos = towerManager.towerBuildPos.transform;

                        // 설치된 상태가 아니면 설치 패널 활성화
                        // 설치된 상태면 업그레이드 패널 활성화
                        if(selectedTowerBuildPos.transform.childCount == 0)
                        {
                            towerBuildPanel.SetActive(true);
                        }
                        else
                        {
                            towerUpgradePanel.SetActive(true);

                            // 선택된 타워 타입으로 업데이트 패널 갱신
                            TowerBase selectedTowerBase = selectedTowerBuildPos.GetChild(0).GetComponent<TowerBase>();
                            upgradePanelTowerImage.sprite = towerSprites[selectedTowerBase.towerType];
                            upgradePanelTowerLvText.text = "타워 레벨 : " + selectedTowerBase.towerLv.ToString();
                            upgradePanelTowerPriceText.text = (selectedTowerBase.towerUpgradeBasicPrice * selectedTowerBase.towerLv).ToString();
                        }

                        // 패널이 활성화된 상태
                        isPanel = true;

                        // 디버깅
                        Debug.Log(selectedTowerBuildPos);
                    }
                }
            }
        }
    }

    // 설치 패널 및 업글 패널 Exit 버튼
    public void ExitPanel(string panelName)
    {
        // 설치 패널 및 업글 패널 닫기
        if(panelName.Equals("build"))
        {
            towerBuildPanel.SetActive(false);
        }
        else if(panelName.Equals("upgrade"))
        {
            towerUpgradePanel.SetActive(false);
        }

        // 패널이 비활성화된 상태
        isPanel = false;
    }

    // 설치할 타워 선택
    public void SelectTower(int towerType)
    {
        // 설치할 타워 프리팹 선택
        selectedTowerPref = towers[(TowerType)towerType].towerPrefabs[0]; 

        // 디버깅
        Debug.Log(selectedTowerPref);
    }

    // 타워 설치
    public void BuildTower()
    {
        // 현재 위치에 타워 생성
        GameObject buildTower = Instantiate(selectedTowerPref, selectedTowerBuildPos.position, Quaternion.identity);

        // 설치된 타워 부모를 선택된 설치 위치로
        buildTower.transform.SetParent(selectedTowerBuildPos);

        // 설치 패널 비활성화
        towerBuildPanel.SetActive(false);

        // 패널 비활성화된 상태
        isPanel = false;
    }

    // 타워 해제
    public void DeleteTower()
    {
        // 자식에 있던 타워 파괴
        Destroy(selectedTowerBuildPos.GetChild(0).gameObject);

        // 업그레이드 패널 비활성화
        towerUpgradePanel.SetActive(false);

        // 패널 비활성화된 상태
        isPanel = false;
    }

    // 타워 업그레이드 
    public void TowerUpgrade()
    {
        // 최대레벨이면 리턴
        if(selectedTowerBuildPos.GetChild(0).GetComponent<TowerBase>().towerLv == 3)
        {
            return;
        }

        // 업그레이드 하면 레벨 및 타워스탯 증가
        selectedTowerBuildPos.GetChild(0).GetComponent<TowerBase>().towerLv++;
        selectedTowerBuildPos.GetChild(0).GetComponent<TowerBase>().basicDamage *= selectedTowerBuildPos.GetChild(0).GetComponent<TowerBase>().towerLv;

        // 선택된 타워 타입으로 업데이트 패널 갱신
        TowerBase selectedTowerBase = selectedTowerBuildPos.GetChild(0).GetComponent<TowerBase>();
        upgradePanelTowerLvText.text = "타워 레벨 : " + selectedTowerBase.towerLv.ToString();
        upgradePanelTowerPriceText.text = (selectedTowerBase.towerUpgradeBasicPrice * selectedTowerBase.towerLv).ToString();

        // 최대레벨이면 텍스트 지움
        if(selectedTowerBuildPos.GetChild(0).GetComponent<TowerBase>().towerLv == 3)
        {
            upgradePanelTowerPriceText.text = "";
        }
    }
}
