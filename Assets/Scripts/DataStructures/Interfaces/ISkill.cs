namespace VHS {
    public enum SkillState {
        None,
        Casting,
        InProgress,
        Finished,
    }

    public enum UseType {
        Instant,
        Custom, // Duration or Infinite
    }
    
    /// <summary>
    /// Zastanów się czy ten interface jest potrzebny bo tylko Skill po nim dziedziczy
    /// </summary>
    public interface ISkill {
        bool FinishSuccessful { get; }
        float CastDuration { get; }
        float SkillDuration { get; }
        
        UseType CastType { get; }
        UseType SkillType { get; }
        SkillState SkillState { get; }
        
        Actor Owner { get; }
        void SetOwner(Actor owner);

        void OnCastStart();
        void OnCastTick(float dt);
        void OnCastFinish();
        void OnCastCancel();
        void FinishCast();
        
        void OnSkillStart();
        void OnSkillTick(float dt);
        void OnSkillFinish();
        void OnSkillCancel();
        void FinishSkill(bool successful);

        void Start();
        void Reset();
        
        void OnReset();
        void OnAbort();
        
        bool CanCastSkill();
    }
}
