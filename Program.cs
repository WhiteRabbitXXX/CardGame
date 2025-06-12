using System;
using System.Text.Json;

namespace _main
{
   class Methods
   {
      public Card[] Shuffle(Card[]? Y)
      {
         for (int x = 0; x<=Y.Length-1; x++)
         {
            Random _rnd = new Random();
            int j = _rnd.Next(x+1);
            Card swp = Y[j];
            Y[j] = Y[x];
            Y[x] = swp;
         }
         
         return Y;
      }
      public void Move(List<Card> _From, int _Index, List<Card> _To)
      {
         _To.Add(_From[_Index]);
         _From.RemoveAt(_Index);
      }
      public bool Compare(List<Card> _AtkDeck, List<Card> _DefDeck, string _Tr)
      {
         int atk = 0;
         int def = 0;
         bool result = true;
         for (int i = 0; i <= _AtkDeck.Count-1; i++)
         {
            if (_AtkDeck[i].Suit == _Tr)
            {
               atk = _AtkDeck[i].Prt + 20;
            }
            else
            {
               atk = _AtkDeck[i].Prt;
            }
            if (_DefDeck[i].Suit == _Tr)
            {
               def = _DefDeck[i].Prt + 20;
            }
            else
            {
               def = _DefDeck[i].Prt;
            }
            if ((atk > def) && ((_DefDeck[i].Suit != _AtkDeck[i].Suit)|(_DefDeck[i].Suit != _Tr)))
            {
               result = false;
            }
            else
            {
               result = true;
            }
         }
         return result;
      }
    //  public bool CheckOut()
    //  {

    //  }
     // public void BotTurn()
     // {
         
    //  }
   }
   class CardGame
   {
      public static void Main()
      {
         // init starts
         var meth = new Methods();
         FileStream fl_strm = new FileStream("card_collection.json", FileMode.OpenOrCreate);
         Card[]? deck = JsonSerializer.Deserialize<Card[]>(fl_strm);
         deck = meth.Shuffle(deck);
         List<Card> lst_deck = deck.Cast<Card>().ToList();
         var Player1 = new List<Card>();
         var Player2 = new List<Card>();
         var AtkTable = new List<Card>();
         var DefTable = new List<Card>();
         for (int i = 0; i < 4; i++)
         {
            meth.Move(lst_deck, 0, Player1);
            meth.Move(lst_deck, 0, Player2);
         }
         string _trump = ($"--Trump is {lst_deck[lst_deck.Count-1].Rank} of {lst_deck[lst_deck.Count-1].Suit}--");
         string _tr = lst_deck[lst_deck.Count-1].Suit;
         bool game_on = true;
         
         // init ends

         // loop start
         while (game_on)
         {
            // update starts
            Console.Clear();
            Console.WriteLine("====enter exit for exit====");
            Console.WriteLine(_trump);
            Console.WriteLine("____Your Cards_____");
            for (int i = 0; i < Player1.Count; i++)
            {
               Console.WriteLine($"[{i+1}] {Player1[i].Rank} of {Player1[i].Suit}");
            }
            Console.WriteLine("______Cards on table______");
            for (int i = 0; i < AtkTable.Count; i++)
            {
               Console.Write($"{i+1} {AtkTable[i].Rank} of {AtkTable[i].Suit}\t");
            }
            Console.WriteLine();
            for (int i = 0; i < DefTable.Count; i++)
            {
               Console.Write($"{i+1} {DefTable[i].Rank} of {DefTable[i].Suit}\t");
            }
            // update ends

            // insert starts
               var insert = Console.ReadLine();
               if (insert == "exit")
               {
                  game_on = false;
               }
               else
               {
                  meth.Move(Player1, Convert.ToInt32(insert)-1, AtkTable);
                  meth.Move(Player2, Convert.ToInt32(insert)-1, DefTable);
                  if (meth.Compare(AtkTable, DefTable, _tr))
                  {
                     Console.WriteLine("yes defend");
                     Console.ReadLine();
                  }
                  else
                  {
                     meth.Move(DefTable, Convert.ToInt32(insert)-1, Player2);
                     Console.WriteLine("no defend");
                     Console.ReadLine();
                  }
               }

            // insert ends

         }
         
         // loop end
      }
   }
   class Card
   {
     public string Suit {get;}
     public string Rank {get;}
     public int Prt {get; set;}
      public Card(string suit, string rank, int prt)
      {
        Suit = suit;
        Rank = rank;
        Prt = prt;
     }
   }
}
