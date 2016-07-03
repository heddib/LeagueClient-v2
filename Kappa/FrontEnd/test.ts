namespace Domain {
    export namespace Masteries {
        export interface Talent {
            index: number;
            level5Desc: string;
            minLevel: number;
            maxRank: number;
            level4Desc: string;
            tltId: number;
            level3Desc: string;
            futureData: Object;
            talentGroupId: number;
            gameCode: number;
            minTier: number;
            prereqTalentGameCode: Object;
            dataVersion: number;
            level2Desc: string;
            name: string;
            talentRowId: number;
            level1Desc: string;
        }
        export interface TalentEntry {
            rank: number;
            talentId: number;
            dataVersion: number;
            talent: Talent;
            summonerId: number;
            futureData: Object;
        }
        export interface MasteryBookPageDTO {
            talentEntries: TalentEntry[];
            dataVersion: number;
            pageId: number;
            name: string;
            current: boolean;
            createDate: Object;
            summonerId: number;
            futureData: Object;
        }
        export interface MasteryBookDTO {
            bookPagesJson: Object;
            dataVersion: number;
            bookPages: MasteryBookPageDTO[];
            dateString: string;
            summonerId: number;
        }
    }

    export namespace Runes {
        export interface RuneType {
            runeTypeId: number;
            dataVersion: number;
            name: string;
            futureData: Object;
        }
        export interface RuneSlot {
            id: number;
            minLevel: number;
            dataVersion: number;
            runeType: RuneType;
            futureData: Object;
        }
        export interface Rune {
            imagePath: Object;
            toolTip: Object;
            tier: number;
            itemId: number;
            runeType: RuneType;
            futureData: Object;
            duration: number;
            gameCode: number;
            itemEffects: any[];
            baseType: string;
            dataVersion: number;
            description: string;
            name: string;
            uses: Object;
        }
        export interface SlotEntry {
            dataVersion: number;
            runeId: number;
            runeSlotId: number;
            runeSlot: RuneSlot;
            rune: Rune;
            futureData: Object;
        }
        export interface SpellBookPageDTO {
            dataVersion: number;
            pageId: number;
            name: string;
            current: boolean;
            slotEntries: SlotEntry[];
            createDate: Date;
            summonerId: number;
            futureData: Object;
        }
        export interface SpellBookDTO {
            bookPagesJson: Object;
            dataVersion: number;
            bookPages: SpellBookPageDTO[];
            dateString: string;
            summonerId: number;
            futureData: Object;
        }
    }

    export namespace MatchHistory {
        export interface PlayerDeltas {
            originalAccountId: number;
            originalPlatformId: string;
            deltas: GameDeltaInfo[];
        }
        export interface GameDeltaInfo {
            gamePlatformId: string;
            gameId: number;
            platformDelta: Delta;
        }
        export interface Delta {
            gamePlatformId: string;
            gameId: number;
            xpDelta: number;
            ipDelta: number;
            compensationModeEnabled: boolean;
            timestamp: number;
        }
        export interface PlayerHistory {
            platformId: string;
            accountId: number;
            shownQueues: number[];
            games: GameResponseInfo;
        }
        export interface GameResponseInfo {
            gameIndexBegin: number;
            gameIndexEnd: number;
            gameTimestampBegin: number;
            gameTimestampEnd: number;
            gameCount: number;
            games: MatchDetails[];
        }
        export interface MatchDetails {
            gameId: number;
            platformId: string;
            gameCreation: number;
            gameDuration: number;
            queueId: number;
            mapId: number;
            seasonId: number;
            gameVersion: string;
            gameMode: string;
            gameType: string;
            teams: Team[];
            participants: Participant[];
            participantIdentities: ParticipantIdentity[];
        }
        export interface Participant {
            participantId: number;
            teamId: number;
            championId: number;
            spell1Id: number;
            spell2Id: number;
            masteries: any[];
            runes: any[];
            stats: ParticipantStats;
            timeline: any;
            highestAchievedSeasonTier: string;
        }
        export interface ParticipantStats {
            participantId: number;
            win: boolean;
            item0: number;
            item1: number;
            item2: number;
            item3: number;
            item4: number;
            item5: number;
            item6: number;
            kills: number;
            deaths: number;
            assists: number;
            largestKillingSpree: number;
            largestMultiKill: number;
            killingSprees: number;
            longestTimeSpentLiving: number;
            doubleKills: number;
            tripleKills: number;
            quadraKills: number;
            pentaKills: number;
            unrealKills: number;
            totalDamageDealt: number;
            magicDamageDealt: number;
            physicalDamageDealt: number;
            trueDamageDealt: number;
            largestCriticalStrike: number;
            totalDamageDealtToChampions: number;
            magicDamageDealtToChampions: number;
            physicalDamageDealtToChampions: number;
            trueDamageDealtToChampions: number;
            totalHeal: number;
            totalUnitsHealed: number;
            totalDamageTaken: number;
            magicalDamageTaken: number;
            physicalDamageTaken: number;
            trueDamageTaken: number;
            goldEarned: number;
            goldSpent: number;
            turretKills: number;
            inhibitorKills: number;
            totalMinionsKilled: number;
            neutralMinionsKilled: number;
            neutralMinionsKilledTeamJungle: number;
            neutralMinionsKilledEnemyJungle: number;
            totalTimeCrowdControlDealt: number;
            champLevel: number;
            visionWardsBoughtInGame: number;
            sightWardsBoughtInGame: number;
            wardsPlaced: number;
            wardsKilled: number;
            firstBloodKill: boolean;
            firstBloodAssist: boolean;
            firstTowerKill: boolean;
            firstTowerAssist: boolean;
            firstInhibitorKill: boolean;
            firstInhibitorAssist: boolean;
            combatPlayerScore: number;
            objectivePlayerScore: number;
            totalPlayerScore: number;
            totalScoreRank: number;
            wasAfk: boolean;
            leaver: boolean;
            gameEndedInEarlySurrender: boolean;
            gameEndedInSurrender: boolean;
            causedEarlySurrender: boolean;
            earlySurrenderAccomplice: boolean;
            teamEarlySurrendered: boolean;
        }
        export interface Team {
            teamId: number;
            win: string;
            firstBlood: boolean;
            firstTower: boolean;
            firstInhibitor: boolean;
            firstBaron: boolean;
            firstDragon: boolean;
            towerKills: number;
            inhibitorKills: number;
            baronKills: number;
            dragonKills: number;
            vilemawKills: number;
            dominionVictoryScore: number;
            bans: { championId: number, pickTurn: number }[];
        }
        export interface ParticipantIdentity {
            participantId: number;
            player: Player;
        }
        export interface Player {
            platformId: number;
            accountId: number;
            summonerName: string;
            summonerId: number;
            currentPlatformId: string;
            currentAccountId: number;
            matchHistoryUri: string;
            profileIcon: number;
        }
    }

    export namespace ActiveGame {
        export interface ActiveGameState {
            ingame: boolean;
            launched: boolean;
        }
    }

    export namespace Summoner {
        export interface PublicSummoner {
            internalName: string;
            dataVersion: number;
            acctId: number;
            name: string;
            profileIconId: number;
            revisionDate: Date;
            revisionId: number;
            summonerLevel: number;
            summonerId: number;
            futureData: Object;
        }
        export interface SummonerKudos {
            friendlies: number;
            helpfuls: number;
            teamworks: number;
            honorables: number;
        }
    }

    export namespace GameData {
        export interface ChampionDetails {
            title: string;
            shortBio: string;
            tacticalInfo: TacticalInfo;
            playstyleInfo: PlaystyleInfo;
            squarePath: string;
            portraitPath: string;
            skins: SkinDetails[];
            passive: SpellDetails;
            spells: SpellDetails[];
            id: number;
            name: string;
            alias: string;
            roles: string[];
        }
        export interface TacticalInfo {
            style: number;
            difficulty: number;
            damageType: string;
        }
        export interface PlaystyleInfo {
            damage: number;
            durability: number;
            crowdControl: number;
            mobility: number;
            utility: number;
        }
        export interface SpellDetails {
            name: string;
            description: string;
        }
        export interface SkinDetails {
            id: number;
            name: string;
            splashPath: string;
            tilePath: string;
            cardPath: string;
            splashVideoPath: string;
            chromaPath: string;
        }
        export interface ChampionSummary {
            id: number;
            name: string;
            alias: string;
            roles: string[];
        }
        export interface ItemDetails {
            id: number;
            name: string;
            description: string;
            price: number;
            priceTotal: number;
        }
        export interface MasteriesInfo {
            type: string;
            tree: MasteryTree;
            data: { [id: string]: Mastery };
        }
        export interface MasteryTree {
            groups: MasteryGroup[];
        }
        export interface MasteryGroup {
            id: number;
            name: string;
            version: number;
            rows: MasteryRow[];
        }
        export interface MasteryRow {
            id: number;
            pointsToActivate: number;
            maxPointsInRow: number;
            masteries: number[];
        }
        export interface Mastery {
            id: number;
            column: string;
            maxRank: number;
            minLevel: number;
            minTier: number;
            name: string;
            description: string[];
            image: MasteryImageInfo;
        }
        export interface MasteryImageInfo {
            icon: string;
        }
        export interface RuneDetails {
            id: number;
            name: string;
            description: string;
            image: string;
            stats: { [id: string]: number };
            rune: RuneInfo;
        }
        export interface RuneInfo {
            tier: number;
            type: string;
        }
        export interface RuneSlots {
            slots: { [id: string]: RuneSlot };
        }
        export interface RuneSlot {
            id: number;
            type: string;
            unlockLevel: number;
        }
        export interface SummonerSpellDetails {
            id: number;
            name: string;
            description: string;
            summonerLevel: number;
            gameModes: string[];
        }
    }

    export namespace ChampionMastery {
        export interface ChampionMasteryDTO {
            highestGrade: string;
            playerId: number;
            championId: number;
            championLevel: number;
            championPoints: number;
            lastPlayTime: number;
            championPointsSinceLastLevel: number;
            championPointsUntilNextLevel: number;
            ChestGranted: boolean;
        }
    }

    //Patcher Service:
    export interface PatcherState {
        phase: string;
        current: number;
        total: number;
    }

    //Collection Service:
    export interface HextechInventory {
        champShards: { [id: number]: number };
        champs: { [id: number]: number };
        skinShards: { [id: number]: number };
        skins: { [id: number]: number };
        wardSkinShards: { [id: number]: number };
        wardSkins: { [id: number]: number };
        mastery6Tokens: { [id: number]: number };
        mastery7Tokens: { [id: number]: number };
        chests: number;
        keys: number;
        keyFragments: number;
        blueEssence: number;
        orangeEssence: number;
    }
    export interface ChampionInventory {
        owned: number[];
        free: number[];
    }
    export interface Skin {
        id: number;
        selected: boolean;
    }

    //Chat Service:
    export interface ChatMessage {
        user: string;
        received: boolean;
        body: string;
        date: Date;
        archived: boolean;
    }
    export interface MucFriend {
        room: string;
        name: string;
    }
    export interface MucMessage {
        room: string;
        from: string;
        body: string;
    }

    //PlayLoop Service:
    export interface AvailableQueue {
        config: number;
        name: string;
        id: number;
    }
    export interface RerollState {
        cost: number;
        points: number;
        maxPoints: number;
    }
    export interface CurrentPlayLoopState {
        inPlayLoop: boolean;
        queueId: number;
        queueConfigId: number;
    }
    export interface ChampSelectState {
        phase: string;
        alliedBans: number[];
        enemyBans: number[];
        isBlue: boolean;
        allies: GameMember[];
        enemies: GameMember[];
        me: GameMember;
        remaining: number;
        turn: number;
        inventory: Inventory;
        reroll: RerollState;
        chatroom: string;
    }
    export interface CustomState {
        blueTeam: LobbyMember[];
        redTeam: LobbyMember[];
        owner: LobbyMember;
        me: LobbyMember;
        chatroom: string;
    }
    export interface GameMember {
        name: string;
        champion: number;
        spell1: number;
        spell2: number;
        id: Object;
        active: boolean;
        trade: string;
        reroll: RerollState;
        intent: boolean;
        role: string;
    }
    export interface Inventory {
        pickableChamps: number[];
        bannableChamps: number[];
        availableSpells: number[];
    }
    export interface LobbyMember {
        name: string;
        id: Object;
        champ: number;
        role1: string;
        role2: string;
    }
    export interface LobbyState {
        isCaptain: boolean;
        canInvite: boolean;
        canMatch: boolean;
        members: LobbyMember[];
        me: LobbyMember;
        chatroom: string;
    }
    export interface MatchmakingState {
        estimate: number;
        actual: number;
        afkCheck: AfkCheck;
        chatroom: string;
    }
    export interface AfkCheck {
        accepted: boolean;
        duration: number;
        remaining: number;
    }
}