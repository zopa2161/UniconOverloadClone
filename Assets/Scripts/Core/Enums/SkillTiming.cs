namespace Core.Enums

{
    [System.Flags]
    public enum SkillTiming
    {
        None = 0,
        OnDeclare = 1<<0, // 0. 시전 선언 (Targeting): "A가 스킬을 쓴다!" (아군 공격 시 버프 등 발동)
        OnTargeting= 1<<1, // 1. 대상 지정의 공격자 시점
        BeforeHit = 1<<2, // 3. 타격 직전 (BeforeAction): 실제 효과 전. (★커버/대신 맞기 최고 명당)
        OnHit= 1<<3, // 4. 타격/효과 적용 (Action): 순수 데미지 적용. (이벤트라기보단 액션 블록 자체)
        AfterHit = 1<<4, // 5. 타격 직후 (Counter): 맞고 난 뒤. (★반격, 피격 시 회복 등 발동)
        //OnFizzle, // 6. 무효화/취소 (deActivated): 타겟이 죽거나 조건이 깨져 취소됨. (Fizzle은 TCG 표준 용어)
        OnSkillEnd= 1<<5 // 7. 스킬 종료 (AfterAction): 완전히 끝남. (스킬 사용 후 AP 페이백 등)
    }
}