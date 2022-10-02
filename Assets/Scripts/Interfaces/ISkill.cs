namespace VHS {
    public enum SkillState {
        None,
        Targetting,
        InProgress,
        Finished,
    }

    public enum SkillType {
        Instant,
        ForDuration,
    }

    public enum CastType {
        SmartCast,
        CustomCast,
    }
    
    /// <summary>
    /// Zastanów się czy ten interface jest potrzebny bo tylko Skill po nim dziedziczy
    /// </summary>
    public interface ISkill {
        float CastDuration { get; }
        float SkillDuration { get; }
        
        CastType CastType { get; }
        SkillType SkillType { get; }
        SkillState SkillState { get; }
        
        Actor Owner { get; }
        void SetOwner(Actor owner);

        void StartTarget();
        void TickTarget(float dt);
        void FinishTarget();
        void CancelTarget();
        
        void StartSkill();
        void TickSkill(float dt);
        void FinishSkill();
        void CancelSkill();

        void Reset();
        void Abort();
        bool CanCastSkill();
    }
}
