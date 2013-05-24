//+------------------------------------------------------------------+
//|                                                  GridPattern.mq4 |
//|                           Copyright © 2012, AirBionicFX Software |
//|                                           http://airbionicfx.com |
//+------------------------------------------------------------------+
#property copyright "Copyright © 2012, AirBionicFX Software"
#property link      "http://airbionicfx.com"

#import "GridPatternLibrary.dll"
   int GetData(string filePath, string fileName, int& legs[], string& actions[], string& error[]);
   int ParseActions(string actionsStr, string& actions[], int& magics[]);
   string GetPattern(int gate, int sameLegCount);
   int IsWatchedPattern(int pattern, string watchedPatterns);
   
   void SaveSession(string appName, string recordId, string &keys[], string &values[], int count);
   int  LoadSession(string appName, string recordId, string &keys[], string &values[], int count); 
   
   void AppendToLog(string appName, int id, string info);
   void CloseLog(int id);
#import

#include <stderror.mqh>
#include <stdlib.mqh>
#include <WinUser32.mqh>

extern string MagicNumber_Help = "Should be unique for each window!!!";
extern int MagicNumber = 42;
extern bool StartNewSession = true;
extern string PatternFile = "Pattern.csv";
extern bool UseLog = true;

extern string LotsSettings = "----------------------------------------------------------------------------------------";
extern double Lots = 1;
extern double LotsX = 1;
extern double LotsY = 1;
extern double LotsZ = 1;
extern bool McMartin = false;
extern string WatchedPatterns = "AKOP";

extern string ColorSettings = "----------------------------------------------------------------------------------------";
extern color EntryColor = Red;
extern color CurrentRunColor = Yellow;
extern color EvenRunColor = Green;
extern color EvenBackColor = LightGray;
extern color OddRunColor = Purple;
extern color OddBackColor = Gray;
extern color ReachablePatternsColor = Red;
extern color ReachedPatternColor = Blue;

#define RowCount 16
#define LegCount 4
#define MagicCount 100

int
   legs[16][4],
   pattern,
   gate,
   bars,
   magic[2],
   entriesTime[5],
   prevRunColor;
   
double
   lots,
   entries[5];
   
string
   errorStr,
   actions[16][5],
   error[1],
   entryLine[2],
   appName,
   recordId;
   
bool
   work,
   isError,
   isNewGate,
   debug;   
      

//+------------------------------------------------------------------+
//| expert initialization function                                   |
//+------------------------------------------------------------------+
int init()
{
   work = true;
   
   if ((MagicNumber <= 0) || (MagicNumber >= 10000))  
      ShowCriticalAlertAndStop("MagicNumber should be more than 0 and less than 10000");

   for (int i = 0; i < 16; i++)
      for (int j = 0; j < 5; j++)
         actions[i][j] = "action example | action example | action example | action example: " + i + j;
   error[0] = "error example | error example | error example | error example | error example: ";
         
   int isLoaded = GetData(TerminalPath(), PatternFile, legs, actions, error);      
      
   if (isLoaded == 0)
      ShowCriticalAlertAndStop("Pattern error: " + error[0]);
      
   if (!work)
      return;
      
   isError = false;  
      
   lots = NormalizeLots(Lots, Symbol());
   magic[OP_BUY] = MagicNumber * MagicCount * 2;                           magic[OP_SELL] = magic[OP_BUY] + MagicCount;
   entryLine[OP_BUY] = "Entry upper line " + Symbol() + "_" + Period();    entryLine[OP_SELL] = "Entry lower line" + Symbol() + "_" + Period();
    
      
   pattern = 0;   
   gate = 0;
   entries[0] = Bid;
   entriesTime[0] = TimeCurrent();
   prevRunColor = Black;
   isNewGate = true;
  
   debug = false;
   appName = WindowExpertName();
   recordId = TerminalName() + "_" + Symbol() + "_" + MagicNumber;
   
   if (!StartNewSession)
      Load();
//----
   return(0);
  }
//+------------------------------------------------------------------+
//| expert deinitialization function                                 |
//+------------------------------------------------------------------+
int deinit()
{
   Save();
   CloseLog(MagicNumber);
   ObjectsDeleteAll(0);
   return(0);
}
  
//+------------------------------------------------------------------+  
void Save()
{
   int dataArrayCount = 13;

   string keys[];
   string values[];
   ArrayResize(keys, dataArrayCount);
   ArrayResize(values, dataArrayCount);
    
   keys[0] = "pattern";                values[0] = pattern;
   keys[1] = "gate";                   values[1] = gate;
   keys[2] = "entries0";               values[2] = DoubleToStr(entries[0], Digits);
   keys[3] = "entries1";               values[3] = DoubleToStr(entries[1], Digits);
   keys[4] = "entries2";               values[4] = DoubleToStr(entries[2], Digits);
   keys[5] = "entries3";               values[5] = DoubleToStr(entries[3], Digits);
   keys[6] = "entries4";               values[6] = DoubleToStr(entries[4], Digits);
   keys[7] = "entriesTime0";           values[7] = entriesTime[0];
   keys[8] = "entriesTime1";           values[8] = entriesTime[1];
   keys[9] = "entriesTime2";           values[9] = entriesTime[2];
   keys[10] = "entriesTime3";          values[10] = entriesTime[3];
   keys[11] = "entriesTime4";          values[11] = entriesTime[4];   
   keys[12] = "lots";                  values[12] = DoubleToStr(lots, Digits); 
      
   SaveSession(appName, recordId, keys, values, dataArrayCount);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void Load()
{
   int dataArrayCount = 13;
   string keys[];
   string values[];
   ArrayResize(keys, dataArrayCount);
   ArrayResize(values, dataArrayCount);  
   for (int i = 0; i < dataArrayCount; i++) 
   {
      keys[i] = "qwertyqwertyqwertyqwertyqwertyqwerty"+  i;
      values[i] = "qwertyqwertyqwertyqwertyqwertyqwerty2" + i;
   }   
   int count = LoadSession(appName, recordId, keys, values, dataArrayCount);
   for (i = 0; i < count; i++)
   {
      if(keys[i] == "pattern") pattern = StrToInteger(values[i]);
      if(keys[i] == "gate") gate = StrToInteger(values[i]);
      if (keys[i] == "entries0") entries[0] = StrToDouble(values[i]);
      if (keys[i] == "entries1") entries[1] = StrToDouble(values[i]);
      if (keys[i] == "entries2") entries[2] = StrToDouble(values[i]);
      if (keys[i] == "entries3") entries[3] = StrToDouble(values[i]);
      if (keys[i] == "entries4") entries[4] = StrToDouble(values[i]);
      if (keys[i] == "entriesTime0") entriesTime[0] = StrToInteger(values[i]);
      if (keys[i] == "entriesTime1") entriesTime[1] = StrToInteger(values[i]);
      if (keys[i] == "entriesTime2") entriesTime[2] = StrToInteger(values[i]);
      if (keys[i] == "entriesTime3") entriesTime[3] = StrToInteger(values[i]);
      if (keys[i] == "entriesTime4") entriesTime[4] = StrToInteger(values[i]);    
      if (keys[i] == "lots") lots = StrToDouble(lots);                                        
   }  
}
//+------------------------------------------------------------------+
  
//+------------------------------------------------------------------+
//| expert start function                                            |
//+------------------------------------------------------------------+
int start()
{   
   if (!IsExpertEnabled()) 
      ShowCriticalAlertAndStop("Expert advisors are disabled for running");

   if (!work) 
   {
      Comment("Reload EA with correct parameters.");
      return;
   }   
   
   if (isError)
   {
		work = false;
		Comment(errorStr);
		return;
   }
   
   for (int side = 0; side < 2; side++)
   {
      if (ProcessEntry(side))
         break;
   }

   if (bars != Bars)
   {
      bars = Bars;
      Redraw();      
   }   

   while (debug);
   return(0);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
bool ProcessEntry(int side)
{
   if (GetOrderCount() == 0)
      ProcessActions();
      
   int entry = GetEntry(side);
   double entryValue = entries[gate] + entry * Point;
   
   bool isEntry = IsEntry(side, entryValue);
   if (IsEntry(side, entryValue))
   {
      SelectNextGate(side);    
      ProcessActions();  
      return (true);
   }
   return (false);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
bool IsEntry(int side, double entryValue)
{
   switch (side)   
   {
      case OP_BUY: return (Bid >= entryValue);
      case OP_SELL: return (Bid <= entryValue);
   }
   return (false);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
int GetEntry(int side)
{
   int entry = -1;
   switch (side)
   {
      case OP_BUY:
         entry = legs[pattern][gate];
         break;
      case OP_SELL:                
         int 
            sameLegCount = GetSameLegCount(gate + 1),
            upperIndex = pattern + sameLegCount;
         if (upperIndex >= RowCount)
            upperIndex = RowCount - 1;
            
         entry = legs[upperIndex][gate];   
   }
   if (entry == -1)
      return (-1);
   return (entry);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void SelectNextGate(int side)
{   
   gate++;        
   if (gate > 4)
   {
      if (McMartin)
         if (IsWatchedPattern(pattern, WatchedPatterns) == 1)
            lots = NormalizeLots(lots * LotsX + LotsY/LotsZ, Symbol());
         else
            lots = NormalizeLots(Lots, Symbol());   
      RedrawRun();
      gate = 0;
      pattern = 0;
      CloseAllOrders();
   }   
   entries[gate] = Bid;
   entriesTime[gate] = TimeCurrent();   
   
   if (gate != 0)
   {
      if (side == OP_SELL)
         pattern += GetSameLegCount(gate);
      string lineName = "Leg line " + gate;
      RedrawLegLine(lineName, entriesTime[gate - 1], entries[gate - 1], entriesTime[gate], entries[gate], CurrentRunColor, 2);
      RedrawReachablePatterns();
   }      
   Redraw(); 
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
string GetActions()
{
   return (actions[pattern][gate]);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void ProcessActions()
{
   string actions = GetActions();
   string currentActions[10];
   int magics[10];
   for (int i = 0; i < 10; i++)
      currentActions[i] = "zxcvbnm,.wsdasasdfghjk" + i;
   int count = ParseActions(actions, currentActions, magics);
   for (int j = 0; j < count; j++)
   {
      if (currentActions[j] == "B")
         OpenTrade(OP_BUY, magics[j]);
      if (currentActions[j] == "S")
         OpenTrade(OP_SELL, magics[j]);
      if (currentActions[j] == "BX")
         CloseTrade(OP_BUY, magics[j]);
      if (currentActions[j] == "SX")
         CloseTrade(OP_SELL, magics[j]);
   }   
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void OpenTrade(int side, int number)
{
   double openPrice;
   color c;
   
   switch (side)
   {
      case OP_BUY:
         openPrice = Ask;
         c = Green;
         break;
      case OP_SELL:
         openPrice = Bid;
         c = Red;
         break;   
   }   

   int ticket = OpenOrder(Symbol(), side, lots, openPrice, 0, 0, 100, NULL, magic[side] + number, 10, 0, c);                 
   if (DisplayError(ticket))
      return;  
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void CloseTrade(int side, int number)
{
   for (int i = OrdersTotal() - 1; i >= 0; i--)
   {
      if (!OrderSelect(i, SELECT_BY_POS))
         continue;
      if (OrderSymbol() != Symbol())   
         continue;
      if (OrderType() != side)   
         continue;
      if (OrderMagicNumber() != (magic[side] + number))
         continue; 
      TryClose(OrderTicket());   
   }     
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void CloseAllOrders()
{
   for (int i = OrdersTotal() - 1; i >= 0; i--)
   {
      if (!OrderSelect(i, SELECT_BY_POS))
         continue;
      if (OrderSymbol() != Symbol())   
         continue;
      if ((OrderMagicNumber() >= magic[OP_BUY]) && (OrderMagicNumber() < (magic[OP_SELL] + MagicCount)))
         TryClose(OrderTicket()); 
   }      
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
int GetSameLegCount(int gate)
{
   switch (gate) 
   {
      case 0: return (16);
      case 1: return (8);
      case 2: return (4);
      case 3: return (2);
      case 4: return (1);
   }
   return (-1);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void Redraw()
{
   for (int side = 0; side < 2; side++)
   {
      int entry = GetEntry(side);   
      DrawEntryLine(side, entry);  
   }      
   RedrawReachablePatterns();
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void RedrawLegLine(string name, int time1, double price1, int time2, double price2, color lineColor, int width)
{  
   string objName = name;   
   ObjectCreate(objName, OBJ_TREND, 0, time1, price1, time2, price2);
   ObjectSet(objName, OBJPROP_COLOR, lineColor);
   ObjectSet(objName, OBJPROP_STYLE, STYLE_SOLID);
   ObjectSet(objName, OBJPROP_WIDTH, width);
   ObjectSet(objName, OBJPROP_RAY, False);   
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void DrawEntryLine(int side, int entry)
{
   double price = entries[gate] + entry*Point;
   int endLine = Time[0] + Period() * 60 * 20;
   string textName = entryLine[side] + " value";
   
   if (ObjectFind(entryLine[side]) == -1)
   {
      ObjectCreate(entryLine[side], OBJ_TREND, 0, Time[0], price, endLine, price);
      ObjectSet(entryLine[side], OBJPROP_COLOR, EntryColor);
      ObjectSet(entryLine[side], OBJPROP_STYLE, STYLE_SOLID);
      ObjectSet(entryLine[side], OBJPROP_WIDTH, 2);
      ObjectSet(entryLine[side], OBJPROP_RAY, False);
   }
      
   ObjectSet(entryLine[side], OBJPROP_TIME1, Time[0]);         
   ObjectSet(entryLine[side], OBJPROP_PRICE1, price);         
   ObjectSet(entryLine[side], OBJPROP_TIME2, endLine);         
   ObjectSet(entryLine[side], OBJPROP_PRICE2, price);         
    
   
   if (ObjectFind(textName) == -1)
      ObjectCreate(textName, OBJ_TEXT, 0, 0, 0);  
            
   ObjectSet(textName, OBJPROP_TIME1, Time[0] + Period() * 60);         
   ObjectSet(textName, OBJPROP_PRICE1, price);     
   ObjectSetText(textName, DoubleToStr(entry, 0), 10, "Times New Roman", EntryColor);  
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void RedrawRun()
{
   color runBackColor;
   switch (prevRunColor)
   {
      case Black: 
      case Purple: 
         prevRunColor = EvenRunColor;
         runBackColor = EvenBackColor; 
         break;
      case Green: 
         prevRunColor = OddRunColor;
         runBackColor = OddBackColor;
         break;
   }
   
   DrawBackground(runBackColor);     
   DrawReachedPattern();
   RemoveCurrentLegLines();   
   for (int i = 1; i < 5; i++)
   {
      string lineName = TimeToStr(TimeCurrent()) + Symbol() + Period() + " gate: " + i;
      RedrawLegLine(lineName, entriesTime[i - 1], entries[i - 1], entriesTime[i], entries[i], prevRunColor, 3);
   }        
   
   string message = "Pattern: " + GetPattern(pattern, 1) +
                    "   Gate0: " + TimeToStr(entriesTime[0]) + " price: " + DoubleToStr(entries[0], Digits) +
                    " --- Gate4: " + TimeToStr(entriesTime[4]) + " price: " + DoubleToStr(entries[4], Digits);
   if (UseLog)                 
      AppendToLog(appName, MagicNumber, message);         
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void DrawReachedPattern()
{
   string textName = "ReachedPattern: " + entriesTime[0] + DoubleToStr(entries[0], Digits); 
   
   int lastBar = iBarShift(Symbol(), Period(), entriesTime[0]);
   int firstBar = iBarShift(Symbol(), Period(), entriesTime[4]);
   
   int timeCoordinate = Time[firstBar + lastBar / 2];
   double priceCoordinate = GetLow(entriesTime[0], entriesTime[4]);
    
   ObjectCreate(textName, OBJ_TEXT, 0, timeCoordinate, priceCoordinate);
   int sameLegCount = GetSameLegCount(4);
   string patterns = GetPattern(pattern, sameLegCount);  
   ObjectSetText(textName, patterns, 16, "Times New Roman", ReachedPatternColor);   
}
//+------------------------------------------------------------------+

void RedrawReachablePatterns()
{
   string textName = "ReachablePatterns"; 
   if (ObjectFind(textName) == -1)  
   {   
      ObjectCreate(textName, OBJ_LABEL, 0, 0, 0);
      ObjectSet(textName, OBJPROP_CORNER, 3); 
      ObjectSet(textName, OBJPROP_XDISTANCE, 10);
      ObjectSet(textName, OBJPROP_YDISTANCE, 10);
   }   
   int sameLegCount = GetSameLegCount(gate);
   string patterns = GetPattern(pattern, sameLegCount);  
   ObjectSetText(textName, "Reachable patterns: " + patterns, 16, "Times New Roman", ReachablePatternsColor);    
}

//+------------------------------------------------------------------+
void RemoveCurrentLegLines()
{
   for (int i = 1; i < 5; i++)
   {
      string lineName = "Leg line " + i;
      if (ObjectFind(lineName) != -1)
         ObjectDelete(lineName);
   }   
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void DrawBackground(color backColor)
{
   string backName = TimeToStr(TimeCurrent()) + Symbol() + Period() + " pattern: " + pattern + " back";
   ObjectCreate(backName, OBJ_RECTANGLE,  0, entriesTime[0], 0, entriesTime[4], 999999);
   ObjectSet(backName, OBJPROP_COLOR, backColor); 
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
double GetLow(int timeStart, int timeEnd)
{
   int startBar = iBarShift(Symbol(), Period(), timeStart);
   int endBar = iBarShift(Symbol(), Period(), timeEnd);
   
   double low = 999999;
   for (int i = endBar; i <= startBar; i++)
   {
      if (i >= Bars)
         break;
      low = MathMin(low, Low[i]);   
   }   
   return (low);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
int GetOrderCount()
{
   int count = 0;
   for (int i = OrdersTotal() - 1; i >= 0; i--)
   {
      if (!OrderSelect(i, SELECT_BY_POS))
         continue;
      if (OrderSymbol() != Symbol())
         continue;
         
      if ((OrderMagicNumber() >= magic[OP_BUY]) && (OrderMagicNumber() < (magic[OP_SELL] + MagicCount)))
         count++;
   } 
   
   return (count);
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
void ShowCriticalAlertAndStop(string alertText)
{
   Alert(alertText);
   work = false;
}
//+------------------------------------------------------------------+

//+------------------------------------------------------------------+
bool TryClose(int ticket) 
{
   if (OrderSelect(ticket, SELECT_BY_TICKET))
   {   
      RefreshRates();
      
      int k = 0;
      while(k < 5)
      {
         RefreshRates();
         if(OrderClose(ticket, OrderLots(), OrderClosePrice(), 100)) 
         {
            return (true);
         }   
         k++;
      }             
   }
   return (false);
}
//+------------------------------------------------------------------+

//+----------------------------------------------------------------------------------------------+
bool DisplayError(int DEerror)
{
   if (DEerror < 0) 
   {
      DEerror = MathAbs(DEerror); 
      errorStr = "Error: " + ErrorDescription(DEerror);
      Alert(errorStr);
      isError = true;
      return (true); 
   }  
   return (false);  
}
//+----------------------------------------------------------------------------------------------+





//+-------------------------+
#define NoneError 0       //|
#define WaitError 1       //|
#define LongWaitError 2   //|
#define StopsError 3      //|
#define CriticalError 4   //|
//+-------------------------+

//+----------------------------------------------------------------------------------------------+
int OpenOrder(string symbol, 
              int type, 
              double lot, 
              double price, 
              int slPoints, 
              int tpPoints, 
              int slippage = 100, 
              string comment = "", 
              int magic = 42, 
              int attempts = 10, 
              datetime expiration = 0, 
              color orderColor = CLR_NONE)
{
   int 
      error,
      ticket;
   
   double  
      stopLevel = MarketInfo(Symbol(), MODE_STOPLEVEL) * Point;
      
   lot = NormalizeLots(lot, Symbol());

   for (int i = 0; i < attempts; i++)
   {
      RefreshRates(); 
      // --> Check price        
      switch (type)
      {
         case OP_BUY:  
            price = Ask; 
            break;
         case OP_SELL: 
            price = Bid; 
            break;
      }
      price = NormalizeDouble(price, Digits);
      // <--      
   
      ticket = OrderSend(symbol, type, lot, price, slippage, 0, 0, comment, magic, expiration, orderColor);
      error = GetLastError() * (-1);    
      switch (ProccessError(error)) 
      {
         case WaitError:
            Sleep(250);
            continue;
         case LongWaitError:
            i--;
            Sleep(100);
            continue;
         case CriticalError: 
            return (error);        
      } 
      break;
   }   
   return (ticket);
}
//+----------------------------------------------------------------------------------------------+

//+----------------------------------------------------------------------------------------------+
int ProccessError(int errorCode)
{
   if (errorCode > 0)
      return (NoneError);
   errorCode = MathAbs(errorCode);   
   switch (errorCode) 
   {
      case ERR_NO_ERROR:
         return (NoneError);
      case ERR_NO_RESULT:
      case ERR_COMMON_ERROR:
      case ERR_INVALID_PRICE:
      case ERR_PRICE_CHANGED:      
      case ERR_TOO_MANY_REQUESTS:
      case ERR_REQUOTE:
         return (WaitError);
      case ERR_TRADE_CONTEXT_BUSY:   
         return (LongWaitError);
      case ERR_INVALID_STOPS:
         return (StopsError);
      default: 
         return (CriticalError);        
   }   
}
//+----------------------------------------------------------------------------------------------+

//+----------------------------------------------------------------------------------------------+
double NormalizeLots(double lots, string symbol)
{
   double lotStep = MarketInfo(symbol, MODE_LOTSTEP),
      maxLot = MarketInfo(symbol, MODE_MAXLOT),
      minLot = MarketInfo(symbol, MODE_MINLOT);
   
   int fullCount = lots / lotStep;            
   double result = fullCount * lotStep;        
   
   if (result < minLot) result = minLot;
   if (result > maxLot) result = maxLot;

   return(result);
}
//+----------------------------------------------------------------------------------------------+