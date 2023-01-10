using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurakForms
{
    internal static class Interface
    {
        static Player current_player;
        static string[] command;
        static bool correctInput = true;
        internal static ServerObject server;
        internal static int id;
        internal static string message;

        static void WrongInput()
        {
            correctInput = false;
          //  server.SendMessage("\nWrong input!", id);
        }

        static internal void ShowTable()
        {
         // server.BroadcastMessage("/c");
       //  message = 
          //  server.BroadcastMessage("Trump: " + GameManager.trump +"\n");           
            for (int i = 0; i < GameManager.players.Count(); i++)
            {
                message = "";
                ShowPlayer(i); 
                ShowCard(GameManager.turnCards[0]);//защищающиеся
                ShowCard(GameManager.turnCards[1]);//атакующие карты        
                if (i == 0)
                    message += GameManager.players[1].cards_count;
                else
                    message += GameManager.players[0].cards_count;
                server.SendMessage(message,i);
            }
        }

        static void ShowPlayer(int id)
        {
            // server.SendMessage("your cards: ", id);
            if (GameManager.players[id].role == Player.Role.Defender)
                message += "false;";
            //   server.SendMessage("[D] ", id);
            else
                message += "true;";
            //  server.SendMessage("[A] ", id);

            ShowCard(GameManager.players[id].cards, id);

        }

        static void ShowCard(List<Card> cards, int id)
        {
         //   int i = 1;
            foreach (var card in cards)
            {
                if (card != null)
                    message += String.Format("{0}{1} ", (int)card.suit,(int) card.rank);
            //    i++;
            }
            message += ";";
          //  server.SendMessage("\n", id);
        }

        static void ShowCard(List<Card> cards)
        {
            int i = 1;
            foreach (var card in cards)
            {
                if (card != null)
                    message += (int)card.suit + (int)card.rank + " ";
                i++;
            }
            message += ";";
            //  server.BroadcastMessage("\n");
        }
        internal static void ProcessingCommand(string command, int _id)
        {
            id = _id;
            string[] _command = command.Split(' ');


            current_player = GameManager.players[id];

            switch (_command[0])
            {
                case "t":
                    switch (current_player.role)
                    {
                        case Player.Role.Attacker:
                            GameManager.ThrowCard(current_player.cards[int.Parse(_command[1]) - 1], current_player);
                            break;
                        case Player.Role.Defender:
                            if (command.Length < 4)
                                GameManager.ThrowCard(current_player.cards[int.Parse(_command[1]) - 1], current_player);
                            else
                                GameManager.ThrowCard(current_player.cards[int.Parse(_command[1]) - 1], current_player, int.Parse(_command[2]) - 1);
                            break;
                        default:
                            WrongInput();
                            break;
                    }
                    break;
                case "e":
                    GameManager.EndTurn(current_player);
                    break;
                default:
                    WrongInput();
                    break;
            }
            correctInput = true;
            Interface.ShowTable();
        }
    }
}