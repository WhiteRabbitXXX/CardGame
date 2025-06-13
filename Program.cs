using System;
using System.Text.Json;

namespace _main
{
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
   class Methods
   {
      public List<Card> Shuffle(List<Card>? Y)
      {
         Random _rnd = new Random();
         for (int x = 0; x<=Y.Count-1; x++)
         {
            int j = _rnd.Next(x+1);
            Card swp = Y[j];
            Y[j] = Y[x];
            Y[x] = swp;
         }
         string trump = Y[Y.Count-1].Suit;
         foreach (Card card in Y)
         {
            if (card.Suit == trump)
            {
               card.Prt += 20;
            }
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
         bool result = true;
         for (int i = 0; i <= _AtkDeck.Count-1; i++)
         {
            if ((_AtkDeck[i].Prt < _DefDeck[i].Prt) && ((_DefDeck[i].Suit == _AtkDeck[i].Suit)|(_DefDeck[i].Suit == _Tr)))
            {
               result = true;
            }
            else
            {
               result = false;
            }
         }
         return result;
      }
    //  public bool CheckOut()
    //  {

    //  }
      public int BotTurn(List<Card> Option, Card Atk, string _Tr)
      {
         var meth = new Methods();
         int result = 0;
         var _AtkDeck = new List<Card>();
         var _DefDeck = new List<Card>();
         Option.Sort((s1, s2) => s1.Prt.CompareTo(s2.Prt));
         _AtkDeck.Add(Atk);
         Console.WriteLine("____Option Cards_____");
         for (int i = 0; i < Option.Count; i++)
         {
            Console.WriteLine($"[{i+1}] {Option[i].Rank} of {Option[i].Suit} ({Option[i].Prt})");
         }
         Console.ReadLine();
         for (int i = 0; (i < Option.Count); i++)
         {
            _DefDeck.Add(Option[i]);
            Console.WriteLine($"{_DefDeck[0].Rank} of {_DefDeck[0].Suit} ({_DefDeck[0].Prt})");
            if (meth.Compare(_AtkDeck, _DefDeck, _Tr))
            {
               result = i;
               Console.WriteLine(result+1);
               Console.ReadLine();
               return result;
            }
            else
            {
               _DefDeck.RemoveAt(0);
            }
         }
         Console.WriteLine("no result");
         Console.ReadLine();
         return result;
      }
   }
   class CardGame
   {
      public static void Main()
      {
         // init starts
         var meth = new Methods();
         FileStream fl_strm = new FileStream("card_collection.json", FileMode.OpenOrCreate);
         List<Card>? deck = JsonSerializer.Deserialize<List<Card>>(fl_strm);
         deck = meth.Shuffle(deck);
         var Player1 = new List<Card>();
         var Player2 = new List<Card>();
         var AtkTable = new List<Card>();
         var DefTable = new List<Card>();
         string _trump = deck[deck.Count-1].Suit;
         for (int i = 0; i < 4; i++)
         {
            meth.Move(deck, 0, Player1);
            meth.Move(deck, 0, Player2);
         }
         
         bool game_on = true;
         
         // init ends

         // loop start
         int round = 0;
         while (game_on)
         {
            // update starts
            
            Console.Clear();
            Console.WriteLine("====enter exit for exit====");
            Console.WriteLine($"Trump is {_trump}");
            Console.WriteLine("____Your Cards_____");
            for (int i = 0; i < Player1.Count; i++)
            {
               Console.WriteLine($"[{i+1}] {Player1[i].Rank} of {Player1[i].Suit} ({Player1[i].Prt})");
            }
            Console.WriteLine("____Bot Cards_____");
            for (int i = 0; i < Player2.Count; i++)
            {
               Console.WriteLine($"[{i+1}] {Player2[i].Rank} of {Player2[i].Suit} ({Player2[i].Prt})");
            }
            
            Console.WriteLine("______Cards on table______");
            for (int i = 0; i < AtkTable.Count; i++)
            {
               Console.Write($"{i+1} {AtkTable[i].Rank} of {AtkTable[i].Suit} ({AtkTable[i].Prt})\t");
            }
            Console.WriteLine();
            for (int i = 0; i < DefTable.Count; i++)
            {
               Console.Write($"{i+1} {DefTable[i].Rank} of {DefTable[i].Suit} ({DefTable[i].Prt})\t");
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
                  meth.Move(Player2, meth.BotTurn(Player2, AtkTable[round], _trump), DefTable);
                  if (meth.Compare(AtkTable, DefTable, _trump))
                  {
                     Console.WriteLine("yes defend");
                     Console.ReadLine();
                  }
                  else
                  {
                     Console.WriteLine("no defend");
                     Console.ReadLine();
                  }
               }

            // insert ends

         }
         
         // loop end
      }
   }
}
