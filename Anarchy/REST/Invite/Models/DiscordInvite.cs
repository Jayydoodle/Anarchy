﻿using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscordInvite : Controllable
    {
        public DiscordInvite()
        {
            OnClientUpdated += (sender, e) =>
            {
                Inviter.SetClient(Client);

                if (Guild != null)
                {
                    ((GuildChannel)Channel).GuildId = Guild.Id;
                    Guild.SetClient(Client);
                }

                Channel.SetClient(Client);
            };
        }


        [JsonProperty("code")]
        public string Code { get; private set; }


        [JsonProperty("channel")]
        [JsonConverter(typeof(DeepJsonConverter<DiscordChannel>))]
        public DiscordChannel Channel { get; private set; }


        [JsonProperty("inviter")]
        public DiscordUser Inviter { get; private set; }


        [JsonProperty("guild")]
        public InviteGuild Guild { get; private set; }


        public InviteType Type
        {
            get { return Guild != null ? InviteType.Guild : InviteType.Group; }
        }


        public async Task DeleteAsync()
        {
            Inviter = (await Client.DeleteInviteAsync(Code)).Inviter;
        }

        /// <summary>
        /// Deletes the invite
        /// </summary>
        /// <returns></returns>
        public void Delete()
        {
            DeleteAsync().GetAwaiter().GetResult();
        }


        public async Task JoinAsync()
        {
            if (Type == InviteType.Guild)
                await Client.JoinGuildAsync(Code);
            else
                await Client.JoinGroupAsync(Code);
        }

        public void Join()
        {
            JoinAsync().GetAwaiter().GetResult();
        }
        

        public override string ToString()
        {
            return Code;
        }
    }
}
