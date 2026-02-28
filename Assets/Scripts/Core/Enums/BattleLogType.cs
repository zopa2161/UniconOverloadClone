namespace Core.Enums
{
    public enum BattleLogType
    {
        SkillDeclare, // 스킬 사용 선언 (이름 외치기, 컷신)
        ApplyEffect,
        Damage, // 데미지 발생 (피격 애니메이션, 숫자 팝업)
        Heal, // 회복 발생 (초록색 숫자 팝업)
        Death // 사망 (쓰러지는 애니메이션)
    }
}