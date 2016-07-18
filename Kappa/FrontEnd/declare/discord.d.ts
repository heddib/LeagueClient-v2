declare type snowflake = string;

declare namespace __Discord {
    namespace Channel {
        interface GuildChannel {
            id: snowflake;
            guild_id: snowflake;
            name: string;
            type: string;
            position: number;
            is_private: boolean;
            permission_overwrites: Overwrite[];
            topic: string;
            last_message_id: snowflake;
            bitrate: number;
        }
        interface DMChannel {
            id: snowflake;
            is_private: boolean;
            recipient: User.User;
            last_message_id: snowflake;
        }
        interface Message {
            id: snowflake;
            channel_id: snowflake;
            author: User.User;
            content: string;
            timestamp: Date;
            edited_timestamp?: Date;
            tts: boolean;
            mention_everyone: boolean;
            mentions: User.User[];
            attachments: Attachment[];
            embeds: Embed[];
            nonce?: number;
        }
        interface Overwrite {
            id: snowflake;
            type: string;
            allow: number;
            deny: number;
        }
        interface Embed {
            title: string;
            type: string;
            description: string;
            url: string;
            thumbnail: EmbedThumbnail;
            provider: EmbedProvider;
        }
        interface EmbedThumbnail {
            url: string;
            proxy_url: string;
            height: number;
            width: number;
        }
        interface EmbedProvider {
            name: string;
            url: string;
        }
        interface Attachment {
            id: snowflake;
            filename: string;
            size: number;
            url: string;
            proxy_url: string;
            height?: number;
            width?: number;
        }
    }

    namespace Guild {
        interface Guild {
            id: snowflake;
            name: string;
            icon: string;
            splash: string;
            owner_id: snowflake;
            region: string;
            afk_channel_id: snowflake;
            afk_timeout: number;
            embed_enabled: boolean;
            embed_channel_id: snowflake;
            verification_level: number;
            voice_states: any[];
            roles: Role[];
            emojis: Emoji[];
            features: string[];

            members: GuildMember[];
        }
        interface UnavailableGuild {
            id: snowflake;
            unavailable: boolean;
        }
        interface GuildEmbed {
            enabled: boolean;
            channel_id: snowflake;
        }
        interface GuildMember {
            user: User.User;
            nick?: string;
            roles: Role[];
            joined_at: Date;
            deaf: boolean;
            mute: boolean;
        }
        interface Integration {
            id: snowflake;
            name: string;
            type: string;
            enabled: boolean;
            syncing: boolean;
            role_id: snowflake;
            expire_behavior: number;
            expire_grace_period: number;
            user: User.User;
            account: IntegrationAccount;
            synced_at: Date;
        }
        interface IntegrationAccount {
            id: string;
            name: string;
        }
        interface Emoji {
            id: snowflake;
            name: string;
            roles: snowflake[];
            require_colons: boolean;
            managed: boolean;
        }
        interface Role {
            id: snowflake;
            name: string;
            color: number;
            hoist: boolean;
            position: number;
            permissions: number;
            managed: boolean;
            mentionable: boolean;
        }
    }

    namespace User {
        interface User {
            id: snowflake;
            username: string;
            discriminator: string;
            avatar: string;
            verified: boolean;
            email: string;
        }
        interface UserGuild {
            id: snowflake;
            name: string;
            icon: string;
            owner: boolean;
            permissions: number;
        }
    }

    namespace Invite {
        interface Invite {
            code: string;
            guild: InviteGuild;
            channel: InviteChannel;
            xkcdpass: string;
        }
        interface InviteMetadata {
            inviter: User.User;
            uses: number;
            max_uses: number;
            max_age: number;
            temporary: boolean;
            created_at: Date;
            revoked: boolean;
        }
        interface InviteGuild {
            id: snowflake;
            name: string;
            splash_hash: string;
        }
        interface InviteChannel {
            id: snowflake;
            name: string;
            type: string;
        }
    }


    namespace Gateway {
        interface Payload {
            d: any;
            op: number;
            t?: string;
            s?: number;
        }

        interface Identity {
            token: string;
            properties: { [id: string]: string };
            compress: boolean;
            large_threshold: number;
            shard: number[];
        }

        interface ReadyData {
            guilds: Guild.Guild[];
            heartbeat_interval: number;
            notes: { [id: string]: string };
            presences: Presence[];
            private_channels: Channel.DMChannel[];
            read_state: any[];
            relationships: Relationship[];
            session_id: string;
            tutorial: any;
            user: User.User;
            user_guild_settings: any[];
            user_settings: any;
            v: number;
        }

        interface PresenceData {
            user: User.User;
            roles: string[];
            game: string;
            guild_id: string;
            status: string;
        }
    }

    interface Presence {
        game: string;
        last_modified: number;
        status: string;
        user: User.User;
    }

    interface Relationship {
        id: string;
        type: number;
        user: User.User;
    }
}

declare module 'discord-domain' {
    export = __Discord;
}