//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AddAutoAttackEffectsInstance
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
using System.Collections.Generic;

public class AddAutoAttackEffectsInstance : AbilityEffectInstance
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	private AddAutoAttackEffectsTemplate m_buffTemplate;
	private float m_duration = 0f;
	private List<AbilityEffectTemplate> m_effectsAdded = new List<AbilityEffectTemplate>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public AddAutoAttackEffectsInstance(AddAutoAttackEffectsTemplate a_template, AbilityContextData a_context) : base(a_template, a_context)
	{
		m_buffTemplate = a_template;
	}

	public override void ApplyEffect()
	{
		base.ApplyEffect();

		m_effectsAdded.Clear();
		foreach (var template in m_buffTemplate.Effects)
		{
			m_effectsAdded.Add(AssetCacher.Instance.CacheAsset<AbilityEffectTemplate>(template));
		}

		var targets = GetTargets();
		foreach (var target in targets)
		{

			target.AddAutoAttackEffects(m_effectsAdded);
		}

		if (m_buffTemplate.Duration == 0f)
			CompleteEffect();


	}

	public override void Update(float a_deltaTime)
	{
		base.Update(a_deltaTime);

		m_duration += a_deltaTime;
		if (m_duration >= m_buffTemplate.Duration)
		{
			RemoveEffects();
			CompleteEffect();
		}
	}

	private void RemoveEffects()
	{
		var targets = GetTargets();
		foreach (var target in targets)
		{
			foreach (var template in m_effectsAdded)
			{
				target.RemoveAutoAttackEffects(template);
			}
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
