using System;
using Altar.Data;
using Altar.Pool;
using Altar.Utilities;
using UnityEngine;

namespace Zyklus.Player
{
	// Token: 0x02000EB4 RID: 3764
	public class PlayerLevelStat : SerializableStatAsset
	{
		// Token: 0x140000EF RID: 239
		// (add) Token: 0x06009176 RID: 37238 RVA: 0x002E8D08 File Offset: 0x002E6F08
		// (remove) Token: 0x06009177 RID: 37239 RVA: 0x002E8D40 File Offset: 0x002E6F40
		public event Action<int> OnLevelGained;

		// Token: 0x06009178 RID: 37240 RVA: 0x002E8D75 File Offset: 0x002E6F75
		private void RaiseOnLevelGained(int level, int gained_levels)
		{
			if (this.OnLevelGained != null)
			{
				this.OnLevelGained(level);
				this.total_ability_point_stat_.AddValue((float)gained_levels);
				this.ability_point_stat_.AddValue((float)gained_levels);
			}
		}

		// Token: 0x06009179 RID: 37241 RVA: 0x002E8DA8 File Offset: 0x002E6FA8
		public override float CalculateValue()
		{
			float num = base.CalculateValue();
			if (Application.isPlaying && (float)this.xp_initialized_level_ != num)
			{
				this.SetXPs((int)num);
			}
			return num;
		}

		// Token: 0x0600917A RID: 37242 RVA: 0x002E8DD6 File Offset: 0x002E6FD6
		public float GetXPNeededForLevelUp()
		{
			return this.xp_for_next_level_ + this.xp_for_this_level_ - this.xp_stat_.GetValue();
		}

		// Token: 0x0600917B RID: 37243 RVA: 0x002E8DF1 File Offset: 0x002E6FF1
		public float GetCurrentXPGap()
		{
			return this.xp_for_next_level_;
		}

		// Token: 0x0600917C RID: 37244 RVA: 0x002E8DF9 File Offset: 0x002E6FF9
		public float GetConsumedXPForCurrentLevel()
		{
			return this.xp_for_this_level_;
		}

		// Token: 0x0600917D RID: 37245 RVA: 0x002E8E01 File Offset: 0x002E7001
		private int CalculateLevel()
		{
			return this.xp_levels_csv_.GetNodeIndexGreaterThanAFloatingValueInAColumn(this.xp_stat_.GetValue(), 'B');
		}

		// Token: 0x0600917E RID: 37246 RVA: 0x0000468E File Offset: 0x0000288E
		protected override void FillDependenciesList(ListPoolInstance<IDependencyNode> dependencies)
		{
		}

		// Token: 0x0600917F RID: 37247
		public void OnXPGain(IStat xp_stat)
		{
			if (xp_stat.GetValue() > (float)this.max_xp_)
			{
				((SerializableStatAsset)xp_stat).SetValue((float)this.max_xp_);
			}
			int num = (int)base.GetValue();
			int num2 =  51;
			if (num >= 51)
			{
				return;
			}
			float value = xp_stat.GetValue();
			if (value >= this.xp_for_next_level_ + this.xp_for_this_level_ || value < this.xp_for_this_level_)
			{
				num2 = this.CalculateLevel();
			}
			if (num != num2)
			{
				base.SetValue((float)num2);
				this.SetXPs(num2);
				DependencyManager.InvalidateNode(this);
				this.RaiseOnLevelGained(num2, num2 - num);
			}
		}

		// Token: 0x06009180 RID: 37248 RVA: 0x002E8EA4 File Offset: 0x002E70A4
		private void SetXPs(int level)
		{
			this.xp_for_this_level_ = this.xp_levels_csv_.GetItemByKey(1, level);
			this.xp_for_next_level_ = this.xp_levels_csv_.GetItemByKey(2, level);
			this.xp_initialized_level_ = level;
		}

		// Token: 0x06009181 RID: 37249 RVA: 0x002E8ED3 File Offset: 0x002E70D3
		public float GetXpRequiredForALevel(int level)
		{
			return this.xp_levels_csv_.GetItemByKey(1, level);
		}

		// Token: 0x06009182 RID: 37250 RVA: 0x002E8EE4 File Offset: 0x002E70E4
		public void FixLevel(SerializableStatAsset xp_stat)
		{
			if (xp_stat.GetValue() > (float)this.max_xp_for_fix)
			{
				xp_stat.SetValue((float)this.max_xp_for_fix);
			}
			int num = this.CalculateLevel();
			base.SetValue((float)num);
			this.SetXPs(num);
			DependencyManager.InvalidateNode(this);
			int num2 = (int)this.total_ability_point_stat_.GetValue();
			if (num - 1 != num2)
			{
				int num3 = num - 1 - num2;
				this.total_ability_point_stat_.AddValue((float)num3);
				this.ability_point_stat_.AddValue((float)num3);
			}
		}

		// Token: 0x04007142 RID: 28994
		private int max_xp_for_fix = 17445357;

		// Token: 0x04007143 RID: 28995
		private int max_xp_ = 28049911;

		// Token: 0x04007144 RID: 28996
		[SerializeField]
		private StatAssetBase xp_stat_;

		// Token: 0x04007145 RID: 28997
		[SerializeField]
		private SerializableStatAsset ability_point_stat_;

		// Token: 0x04007146 RID: 28998
		[SerializeField]
		private SerializableStatAsset total_ability_point_stat_;

		// Token: 0x04007147 RID: 28999
		[SerializeField]
		private CSVAsset xp_levels_csv_;

		// Token: 0x04007148 RID: 29000
		private const int kLevelCap = 51;

		// Token: 0x0400714A RID: 29002
		private int xp_initialized_level_;

		// Token: 0x0400714B RID: 29003
		private float xp_for_this_level_;

		// Token: 0x0400714C RID: 29004
		private float xp_for_next_level_;
	}
}
