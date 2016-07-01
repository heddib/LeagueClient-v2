interface ChampionListDto {
    type: string;
    format: string;
    version: string;
    data: { [id: string]: ChampionDto };
    keys: { [id: number]: string };
}
interface ChampionDto {
    version: string;
    id: string;
    key: string;
    name: string;
    title: string;
    image: ImageDto;
    skins: SkinDto[];
    lore: string;
    blurb: string;
    allytips: string[];
    enemytips: string[];
    tags: string[];
    partype: string;
    info: RatingsDto;
    stats: StatsDto;
    spells: SpellDto[];
    passive: PassiveDto;
    recommended: any[];
}
interface ImageDto {
    full: string;
    sprite: string;
    group: string;
    x: number;
    y: number;
    w: number;
    h: number;
}
interface ItemListDto {
    type: string;
    version: string;
    basic: ItemDto;
    data: { [id: number]: ItemDto };
    groups: GroupDto[];
    tree: TreeDto[];
}
interface ItemDto {
    name: string;
    rune: RuneDto;
    gold: CostDto;
    group: string;
    description: string;
    colloq: string;
    plaintext: string;
    image: ImageDto;
    consumed: boolean;
    stacks: number;
    depth: number;
    consumeOnFull: boolean;
    from: string[];
    into: string[];
    specialRecipe: number;
    inStore: boolean;
    hideFromAll: boolean;
    requiredChampion: string;
    stats: StatsDto;
    tags: string[];
    maps: { [id: string]: boolean };
}
interface MapListDto {
    type: string;
    version: string;
    data: { [id: string]: MapDto };
}
interface MapDto {
    MapName: string;
    MapId: string;
    UnpurchasableItemList: string[];
    image: ImageDto;
}
interface MasteryListDto {
    type: string;
    version: string;
    tree: MasteryTreeDto;
    data: { [id: number]: MasteryDto };
}
interface MasteryTreeDto {
    Ferocity: BranchDto[][];
    Cunning: BranchDto[][];
    Resolve: BranchDto[][];
}
interface MasteryDto {
    id: number;
    name: string;
    description: string[];
    image: ImageDto;
    ranks: number;
    prereq: string;
}
interface ProfileIconListDto {
    type: string;
    version: string;
    data: { [id: number]: ProfileIconDto };
}
interface ProfileIconDto {
    id: number;
    image: ImageDto;
}
interface SpellListDto {
    type: string;
    version: string;
    data: { [id: string]: SpellDto };
}
interface SpellDto {
    id: string;
    name: string;
    description: string;
    tooltip: string;
    leveltip: LevelTipDto;
    maxrank: number;
    cooldown: number[];
    cooldownBurn: string;
    cost: number[];
    costBurn: string;
    effect: number[][];
    effectBurn: string[];
    vars: ScalingDto[];
    key: string;
    summonerLevel: number;
    modes: string[];
    costType: string;
    maxammo: string;
    rangeBurn: string;
    image: ImageDto;
    resource: string;
}
interface ScalingDto {
    link: string;
    key: string;
    coeff: number[];
}
interface LevelTipDto {
    label: string[];
    effect: string[];
}
interface RatingsDto {
    attack: number;
    defense: number;
    magic: number;
    difficulty: number;
}
interface StatsDto {
    hp: number;
    hpperlevel: number;
    mp: number;
    mpperlevel: number;
    movespeed: number;
    armor: number;
    armorperlevel: number;
    spellblock: number;
    spellblockperlevel: number;
    attackrange: number;
    hpregen: number;
    hpregenperlevel: number;
    mpregen: number;
    mpregenperlevel: number;
    crit: number;
    critperlevel: number;
    attackdamage: number;
    attackdamageperlevel: number;
    attackspeedoffset: number;
    attackspeedperlevel: number;
}
interface SkinDto {
    id: string;
    num: number;
    name: string;
    chromas: boolean;
}
interface PassiveDto {
    name: string;
    description: string;
    image: ImageDto;
}
interface RuneDto {
    isrune: boolean;
    tier: number;
    type: string;
}
interface CostDto {
    base: number;
    total: number;
    sell: number;
    purchasable: boolean;
}
interface StatsDto {
    FlatHPPoolMod: number;
    rFlatHPModPerLevel: number;
    FlatMPPoolMod: number;
    rFlatMPModPerLevel: number;
    PercentHPPoolMod: number;
    PercentMPPoolMod: number;
    FlatHPRegenMod: number;
    rFlatHPRegenModPerLevel: number;
    PercentHPRegenMod: number;
    FlatMPRegenMod: number;
    rFlatMPRegenModPerLevel: number;
    PercentMPRegenMod: number;
    FlatArmorMod: number;
    rFlatArmorModPerLevel: number;
    PercentArmorMod: number;
    rFlatArmorPenetrationMod: number;
    rFlatArmorPenetrationModPerLevel: number;
    rPercentArmorPenetrationMod: number;
    rPercentArmorPenetrationModPerLevel: number;
    FlatPhysicalDamageMod: number;
    rFlatPhysicalDamageModPerLevel: number;
    PercentPhysicalDamageMod: number;
    FlatMagicDamageMod: number;
    rFlatMagicDamageModPerLevel: number;
    PercentMagicDamageMod: number;
    FlatMovementSpeedMod: number;
    rFlatMovementSpeedModPerLevel: number;
    PercentMovementSpeedMod: number;
    rPercentMovementSpeedModPerLevel: number;
    FlatAttackSpeedMod: number;
    PercentAttackSpeedMod: number;
    rPercentAttackSpeedModPerLevel: number;
    rFlatDodgeMod: number;
    rFlatDodgeModPerLevel: number;
    PercentDodgeMod: number;
    FlatCritChanceMod: number;
    rFlatCritChanceModPerLevel: number;
    PercentCritChanceMod: number;
    FlatCritDamageMod: number;
    rFlatCritDamageModPerLevel: number;
    PercentCritDamageMod: number;
    FlatBlockMod: number;
    PercentBlockMod: number;
    FlatSpellBlockMod: number;
    rFlatSpellBlockModPerLevel: number;
    PercentSpellBlockMod: number;
    FlatEXPBonus: number;
    PercentEXPBonus: number;
    rPercentCooldownMod: number;
    rPercentCooldownModPerLevel: number;
    rFlatTimeDeadMod: number;
    rFlatTimeDeadModPerLevel: number;
    rPercentTimeDeadMod: number;
    rPercentTimeDeadModPerLevel: number;
    rFlatGoldPer10Mod: number;
    rFlatMagicPenetrationMod: number;
    rFlatMagicPenetrationModPerLevel: number;
    rPercentMagicPenetrationMod: number;
    rPercentMagicPenetrationModPerLevel: number;
    FlatEnergyRegenMod: number;
    rFlatEnergyRegenModPerLevel: number;
    FlatEnergyPoolMod: number;
    rFlatEnergyModPerLevel: number;
    PercentLifeStealMod: number;
    PercentSpellVampMod: number;
}
interface GroupDto {
    id: string;
    MaxGroupOwnable: string;
}
interface TreeDto {
    header: string;
    tags: string[];
}
interface BranchDto {
    masteryId: string;
    prereq: string;
}
