﻿using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System;
using System.IO;
using Newtonsoft.Json;

namespace Manderville.Modules {
	public class HelpModule : ModuleBase<SocketCommandContext> {
		private CommandService _service;

		// Create a constructor for the CommandService dependency
		public HelpModule(CommandService service) {
			_service = service;
		}

       

        [Command("help")]
        [Alias("h")]
        [Summary("Lists all commands")]
        public async Task HelpAsync() {
            var application = await Context.Client.GetApplicationInfoAsync();
            string reply = $"__**Available Commands**__\n" +
                $"**a**ssign `server` `character name`:  Assigns a user roles depending on their fflogs rankings. \n\n" +
                $"**Contact**\n" +
                $"Please send all feature suggestions and bot problems to: " +
                $"{application.Owner.Mention} ";

           
            var embed = new EmbedBuilder()
                .WithColor(new Color(250, 140, 73))
                .WithTitle("Help")
                .WithFooter(new EmbedFooterBuilder().WithText($"Manderville v0.1.0"))
                .WithDescription(reply)
                .WithUrl("https://gist.github.com/Infinifrui/88e578a66df698fcb27d418940f7c680")
                .Build();


            await ReplyAsync("", embed: embed);
        }

        [Command("help")]
		[Alias("h")]
		[Summary("Lists help for specified command")]
		public async Task HelpAsync([Remainder]string input) {
            var result = from Module in _service.Modules
                         from command in Module.Commands
                         where command.Name.ToLower().Contains(input.ToLower()) && Module.Name != "RecipeMisc"
                         select command;

            var builder = new EmbedBuilder() {
                Color = new Color(250, 140, 73),
                Description = $"Here are some commands like **{input}**"
            };

            foreach (var cmd in result) {
                Console.WriteLine($"Input: {input}\nCommand: {cmd.Name}\nmatch: {input.ToLower() == cmd.Name.ToLower()}");
                if (cmd.Remarks != null) {
                    builder.AddField(x => {
                        
                        x.Name = cmd.Module.Name + ": " + string.Join(", ", cmd.Name);
                        x.Value = $"Aliases: `{string.Join(", ", cmd.Aliases)}`\nSummary: {cmd.Summary}\n";
                    });
                }
                
                Console.WriteLine("Builder: Field Added");
            }

            builder.Build();

            if (result == null) {
				await ReplyAsync($"Sorry, I couldn't find a command like **{input}**");
				return;
			}

			await ReplyAsync("", embed: builder);
		}














    }
}
