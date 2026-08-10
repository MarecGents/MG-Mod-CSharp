// 核对 config.json / defaultConfig.json 与 ConfigSettingType.cs 的字段一致性
const fs = require('fs');
const path = 'MGmod/res/config/';
const j1 = JSON.parse(fs.readFileSync(path + 'config.json', 'utf8'));
const j2 = JSON.parse(fs.readFileSync(path + 'defaultConfig.json', 'utf8'));

// C# 属性清单（从 ConfigSettingType.cs 手工提取的完整映射）
const cs = {
  Bot: ['AIHealth', 'BotSystem'],
  'Bot.BotSystem': ['BotBrain', 'EquipmentQuality', 'BotNameAdd'],
  'Bot.BotSystem.BotBrain': ['enable', 'type', 'customBotBrain'],
  Config: ['AirdropType', 'AISpawnNumber', 'RaidDefault', 'ReturnChance', 'BuyFoundInRaid', 'LootMultiple', 'RandomContainer', 'USECRate', 'Sell100', 'SellFast', 'SellOptimize', 'SellNew', 'NoBlackList', 'Buffs', 'UpdateTime', 'WeatherSettings', 'NoLostonDeath', 'ScavEquipmentOptimize', 'BotSystem', 'GiftsAdd'],
  'Config.RaidDefault': ['enable', 'aiAmount', 'aiDifficulty', 'bossEnabled', 'scavWars', 'taggedAndCursed', 'enablePve', 'randomWeather', 'randomTime'],
  'Config.BotSystem': ['PmcWavesOptimize'],
  Globals: ['EscapeNoTimeLimit', 'FleaMarketOpenLevel', 'TakeLimit', 'ScavOptimize', 'LowTaxRate', 'SellNumber', 'LoadSpeed', 'SuperHero', 'LootMultiplier', 'ArmorRepairPerfect', 'Buffs', 'ExpOptimize'],
  'Globals.LoadSpeed': ['mode', 'BaseLoadTime', 'BaseUnloadTime'],
  'Globals.LootMultiplier': ['Value', 'Global'],
  Hideout: ['BuildTime', 'ProductTime', 'ScavCaseTime', 'UpgradeNoLimit', 'BonusesLevel', 'NoNeedsFuel', 'Qte'],
  'Hideout.Qte': ['Sucess100', 'NoPunish', 'RewardMultiple'],
  Locations: ['RaidTime', 'BOSSSpwanChance', 'Pass100', 'Escape100', 'MapInsurance', 'BotSystem'],
  'Locations.BotSystem': ['ScavWavesOptimize', 'MapRefershConfig', 'PmcTacticalSquad', 'MapBotDifficulty'],
  Templates: ['Examined', 'WeaponFilter', 'AmmoStack', 'AmmoInfo', 'ContainerExpand', 'Safes', 'MoneyStack', 'Backpack', 'Armor', 'Helmet', 'EquipmentPlate', 'KeysDurability', 'MedcDurability', 'WeaponNoLost', 'WeaponRepairPerfect', 'MagazineCapacity', 'T7ThermalImaging', 'ResetFree', 'QuestSystem', 'PMCRoar'],
  'Templates.QuestSystem': ['QuestOptimize'],
  Traders: ['InsuranceTime', 'InsuranceCost'],
  MGCustom: ['CustomTrader', 'CustomItem', 'CustomAssort', 'CustomProfile', 'CustomBoss', 'KeyClassfy', 'SyncFlea', 'SeasonalActivity'],
  'MGCustom.SeasonalActivity': ['enable', 'AcitvitiesSwitch', 'NewActivitiesSwitch'],
};

// 从 JSON 提取指定路径的字段集合（点号路径）
function getJsonKeys(obj, path) {
  const parts = path.split('.');
  let cur = obj;
  for (const p of parts) {
    if (cur && typeof cur === 'object' && p in cur) cur = cur[p];
    else return [];
  }
  if (cur && typeof cur === 'object' && !Array.isArray(cur)) return Object.keys(cur);
  return [];
}

function compare(name, json) {
  console.log('========== ' + name + ' ==========');
  let issues = 0;
  for (const [sec, csKeys] of Object.entries(cs)) {
    const jKeys = getJsonKeys(json, sec);
    const onlyCs = csKeys.filter(k => !jKeys.includes(k));
    const onlyJson = jKeys.filter(k => !csKeys.includes(k));
    if (onlyCs.length || onlyJson.length) {
      issues++;
      console.log('  [' + sec + ']');
      if (onlyJson.length) console.log('    JSON 有但 C# 无: ' + onlyJson.join(', '));
      if (onlyCs.length) console.log('    C# 有但 JSON 无: ' + onlyCs.join(', '));
    }
  }
  // 顶层检查
  const topCs = ['Bot', 'Config', 'Globals', 'Hideout', 'Locations', 'Templates', 'Traders', 'MGCustom', 'saveTime'];
  const topJson = Object.keys(json);
  const onlyTopCs = topCs.filter(k => !topJson.includes(k));
  const onlyTopJson = topJson.filter(k => !topCs.includes(k));
  if (onlyTopCs.length || onlyTopJson.length) {
    issues++;
    console.log('  [顶层]');
    if (onlyTopJson.length) console.log('    JSON 有但 C# 无: ' + onlyTopJson.join(', '));
    if (onlyTopCs.length) console.log('    C# 有但 JSON 无: ' + onlyTopCs.join(', '));
  }
  if (issues === 0) console.log('  ✓ 全部一致（无差异）');
  return issues;
}

const i1 = compare('config.json', j1);
const i2 = compare('defaultConfig.json', j2);

// config.json vs defaultConfig.json 结构差异
console.log('========== config.json vs defaultConfig.json 结构 ==========');
const diffKeys = [];
for (const [sec, csKeys] of Object.entries(cs)) {
  const a = getJsonKeys(j1, sec).sort();
  const b = getJsonKeys(j2, sec).sort();
  if (JSON.stringify(a) !== JSON.stringify(b)) diffKeys.push(sec + ': config=' + a.join(',') + ' | default=' + b.join(','));
}
if (diffKeys.length) diffKeys.forEach(d => console.log('  差异: ' + d));
else console.log('  ✓ 两 JSON 结构完全一致');
console.log('config.json 问题数: ' + i1);
console.log('defaultConfig.json 问题数: ' + i2);
