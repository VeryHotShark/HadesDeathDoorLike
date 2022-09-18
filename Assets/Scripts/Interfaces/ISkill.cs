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
    
    public interface ISkill {
        float CastDuration { get; }
        float SkillDuration { get; }
        
        CastType CastType { get; }
        SkillType SkillType { get; }
        SkillState SkillState { get; }
        
        Actor Owner { get; }
        void SetOwner(Actor owner);
        void SetState(SkillState state);

        void StartTarget();
        void TickTarget(float dt);
        void FinishTarget();
        void StartSkill();
        void TickSkill(float dt);
        void FinishSkill();
        bool CanCastSkill();
    }
}
