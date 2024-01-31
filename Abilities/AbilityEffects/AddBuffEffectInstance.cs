//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AbilityInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class AddBuffEffectInstance : AbilityEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected AddBuffEffectTemplate m_buffTemplate;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public AddBuffEffectInstance(AddBuffEffectTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_buffTemplate = a_template;
	}

	public override void ApplyEffect()
	{
		base.ApplyEffect();

		var targets = GetTargets();
		foreach (var target in targets)
		{
			foreach (var buffTemplate in m_buffTemplate.BuffsAdded)
			{
				float randomChance = UnityEngine.Random.Range(0f, 1f);
				if (m_buffTemplate.Chance >= randomChance)
				{
					if (buffTemplate.CanReapplyBuff || (!buffTemplate.CanReapplyBuff && !DoesTargetHaveBuffAlready(target, buffTemplate)))
					{
						BuffContextData context = new BuffContextData();
						context.Source = m_context.Source;
						context.Target = target;
						context.Duration = m_buffTemplate.Duration;
						context.RemoveTrigger = m_buffTemplate.RemoveTriggerCondition;
						var newBuff = context.Target.AddBuff(context, buffTemplate, m_buffTemplate.StackAmount);
					}
				}
			}
		}
		CompleteAddBuff();
	}

	protected virtual void CompleteAddBuff()
	{
		CompleteEffect();
	}

	private bool DoesTargetHaveBuffAlready(UnitInstance a_target, BuffTemplate a_buffTemplate)
	{
		foreach (var buff in a_target.CurrentBuffs)
		{
			if (!buff.ReadyToRemove && buff.Template.TID == a_buffTemplate.TID)
			{
				return true;
			}
		}
		return false;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
