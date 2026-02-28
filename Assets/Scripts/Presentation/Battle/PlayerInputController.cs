using UnityEngine;

namespace Presentation.Battle
{
public class PlayerInputController : MonoBehaviour
{
    public Squad selectedSquad;    
    
    [Header("Raycast Layers")]
    public LayerMask squadLayer;   
    public LayerMask groundLayer;  // 2D 맵의 이동 가능한 영역

    private void Update()
    {
        HandleSelection();
        HandleMovementCommand();
    }

    private void HandleSelection()
    {
        // 마우스 좌클릭 (0)
        if (Input.GetMouseButtonDown(0)) 
        {
            // 1. 마우스 화면 좌표를 2D 월드 좌표로 변환
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 2. 2D 레이캐스트 (마우스가 찍힌 지점의 오브젝트 검출)
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, squadLayer);

            if (hit.collider != null)
            {
                Squad clickedSquad = hit.collider.GetComponent<Squad>();
                if (clickedSquad != null)
                {
                    // 기존 부대 선택 해제
                    if (selectedSquad != null) selectedSquad.SetSelected(false);

                    // 새 부대 선택
                    selectedSquad = clickedSquad;
                    selectedSquad.SetSelected(true);
                }
            }
            else 
            {
                // 허공이나 땅을 클릭하면 선택 해제
                if (selectedSquad != null)
                {
                    selectedSquad.SetSelected(false);
                    selectedSquad = null;
                }
            }
        }
    }

    private void HandleMovementCommand()
    {
        // 마우스 우클릭 (1) + 선택된 부대가 있을 때만
        if (Input.GetMouseButtonDown(1) && selectedSquad != null) 
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 바닥(Ground) 콜라이더가 있는 곳만 이동 가능하게 처리
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f, groundLayer);

            if (hit.collider != null)
            {
                // 클릭한 곳이 Ground 레이어라면 그 위치로 이동 명령
                selectedSquad.SetTarget(worldPos);
            }
            
            /* 💡 만약 맵 전체가 제한 없는 이동 공간이라면, 
               위의 if (hit.collider != null) 부분을 지우고 
               그냥 바로 selectedSquad.SetTarget(worldPos); 를 호출하면 돼! */
        }
    }
}
}