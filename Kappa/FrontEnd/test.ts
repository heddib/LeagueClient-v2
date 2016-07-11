namespace Domain {
    export namespace Assets {
        export interface AssetsInfo {
            version: string;
            locale: string;
        }
    }
    export namespace Authentication {
        export interface AccountState {
            inGame: boolean;
        }
        export interface QueuePosition {
            position: number;
        }
        export interface SavedAccount {
            user: string;
            name: string;
            icon: number;
        }
    }
    export namespace Chat {
        export interface ChatMessage {
            user: string;
            body: string;
            received: boolean;
            date: Date;
            archived: boolean;
        }
        export interface MucFriend {
            room: string;
            user: string;
            name: string;
        }
        export interface MucMessage {
            room: string;
            from: string;
            body: string;
        }
    }
    export namespace Collection {
        export interface ChampionInventory {
            owned: number[];
            free: number[];
        }
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
        export interface MasteryBook {
            selected: number;
            pages: MasteryPage[];
        }
        export interface MasteryPage {
            id: number;
            name: string;
            masteries: { [id: string]: number };
        }
        export interface RuneBook {
            selected: number;
            pages: RunePage[];
        }
        export interface RunePage {
            id: number;
            name: string;
            runes: { [id: string]: number };
        }
        export interface Skin {
            id: number;
            selected: boolean;
        }
    }
    export namespace Diagnostics {
        export interface Versions {
            game: string;
            patch: string;
        }
    }
    export namespace Game {
        export interface ActiveGameState {
            launched: boolean;
        }
        export interface AfkCheck {
            accepted: boolean;
            duration: number;
            remaining: number;
        }
        export interface AvailableQueue {
            config: number;
            name: string;
            id: number;
            map: number;
        }
        export interface ChampionMasteryState {
            level: number;
            totalPoints: number;
            pointsInLevel: number;
            pointsSinceLevel: number;
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
        export interface PlayLoopState {
            inPlayLoop: boolean;
            queueId: number;
            queueConfigId: number;
        }
        export interface PostGameChampionMastery {
            before: ChampionMasteryState;
            after: ChampionMasteryState;
            grade: string;
            champion: number;
        }
        export interface PostGameState {
            chatroom: string;
            ipEarned: number;
            ipTotal: number;
            ipLifetime: number;
            hextech: Collection.HextechInventory;
            championMastery: PostGameChampionMastery;
        }
        export interface RerollState {
            cost: number;
            points: number;
            maxPoints: number;
        }
    }
    export namespace Patcher {
        export interface PatcherState {
            phase: string;
            current: number;
            total: number;
        }
    }
    export namespace Summoner {
        export interface SummonerKudos {
            friendlies: number;
            helpfuls: number;
            teamworks: number;
            honorables: number;
        }
        export interface SummonerSummary {
            internalName: string;
            summonerId: number;
            accountId: number;
            name: string;
            level: number;
            icon: number;
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
        export interface ChampionSummary {
            id: number;
            name: string;
            alias: string;
            roles: string[];
        }
        export interface ChromaDetails {
            id: number;
            name: string;
            chromaPath: string;
            cardPath: string;
            colors: string[];
        }
        export interface ItemDetails {
            id: number;
            name: string;
            description: string;
            price: number;
            priceTotal: number;
        }
        export interface MapSummary {
            id: number;
            name: string;
            description: string;
        }
        export interface MasteriesInfo {
            type: string;
            tree: MasteryTree;
            data: { [id: string]: Mastery };
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
        export interface MasteryGroup {
            id: number;
            name: string;
            version: number;
            rows: MasteryRow[];
        }
        export interface MasteryImageInfo {
            icon: string;
        }
        export interface MasteryRow {
            id: number;
            pointsToActivate: number;
            maxPointsInRow: number;
            masteries: number[];
        }
        export interface MasteryTree {
            groups: MasteryGroup[];
        }
        export interface PlaystyleInfo {
            damage: number;
            durability: number;
            crowdControl: number;
            mobility: number;
            utility: number;
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
        export interface RuneSlot {
            id: number;
            type: string;
            unlockLevel: number;
        }
        export interface RuneSlots {
            slots: { [id: string]: RuneSlot };
        }
        export interface SkinDetails {
            id: number;
            name: string;
            splashPath: string;
            tilePath: string;
            cardPath: string;
            splashVideoPath: string;
            chromaPath: string;
            chromas: ChromaDetails[];
        }
        export interface SpellDetails {
            name: string;
            description: string;
        }
        export interface SummonerSpellDetails {
            id: number;
            name: string;
            description: string;
            summonerLevel: number;
            gameModes: string[];
        }
        export interface TacticalInfo {
            style: number;
            difficulty: number;
            damageType: string;
        }
        export interface WardSkinSummary {
            id: number;
            name: string;
            description: string;
            wardImagePath: string;
            wardShadowImagePath: string;
        }
    }
    export namespace MatchHistory {
        export interface Delta {
            gamePlatformId: string;
            gameId: number;
            xpDelta: number;
            ipDelta: number;
            compensationModeEnabled: boolean;
            timestamp: number;
        }
        export interface GameDeltaInfo {
            gamePlatformId: string;
            gameId: number;
            platformDelta: Delta;
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
        export interface ParticipantIdentity {
            participantId: number;
            player: Player;
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
        export interface PlayerDeltas {
            originalAccountId: number;
            originalPlatformId: string;
            deltas: GameDeltaInfo[];
        }
        export interface PlayerHistory {
            platformId: string;
            accountId: number;
            shownQueues: number[];
            games: GameResponseInfo;
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
            bans: any[];
        }
    }
}
