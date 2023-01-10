using System.Net.Sockets;
using System.Text;

namespace DurakForms
{
    public class Player
    {
        public List<Card> cards = new List<Card>(6);
        public int cards_count => cards.Count();

        public Role role;

        public readonly string nickname;



        public Player(List<Card> cards)
        {
            this.cards = cards;
        }

        public enum Role
        {
            Defender, Attacker,
        }

        public void SwitchRole()
        {
            switch (role)
            {
                case Role.Defender: role = Role.Attacker; break;
                case Role.Attacker: role = Role.Defender; break;
            }
        }


        public void AddCards(List<Card> additional_cards)
        {
            cards.AddRange(additional_cards);
        }
        public void AddCards(Card additional_card)
        {
            cards.Add(additional_card);
        }

    }

    public class Card
    {
        public readonly Suit suit;
        public readonly Rank rank;
        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }


        public enum Suit
        {
            Diamonds,
            Hearts,
            Clubs,
            Spades,
        }

        public enum Rank
        {
            Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace,
        }
    }
}