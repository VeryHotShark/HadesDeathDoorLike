﻿using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace VHS{
	[Category("Skills")]
	[Description("Cast Chosen Skill")]
	public class CastSkill : ActionTask<SkillCasterComponent> {
		[ReferenceField] public NpcSkill _skill;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			agent.InitSkill(_skill);
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			bool canCastSkill = agent.CastSkill(_skill);
			
			if(!canCastSkill)
				EndAction(false);
				
			if(_skill.SkillState == SkillState.Finished)
				EndAction(true);
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate(){
			agent.TickSkill(Time.deltaTime);

			if (_skill.SkillState == SkillState.Finished) {
				Debug.Log("DUPA2");	
				EndAction(true);
			}
		}

		protected override void OnStop(bool interrupted) {
			if(interrupted)
				agent.CancelSkill();
			
			OnStop();
		}

		//Called when the task is disabled.
		protected override void OnStop() => _skill.Reset();
		
#if UNITY_EDITOR
		protected override void OnTaskInspectorGUI() {
			base.OnTaskInspectorGUI();
			
		}
#endif
	}
}