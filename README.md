# TradingBot

This project is an exercise in using various .NET tools within the context of a non-game-related application (as my previous focus has primarily been on game development).

Current status: Work in progress (early stage)

***Important Note:*** The [BotSimulation](./Library/Bot/BotSimulation.cs) class currently forms the core of the system. As most supporting modules are being designed around its logic, it's the best point of reference for understanding how the bot operates and evolves.

### 📝 Implementation Checklist

| Feature                                    | Implemented |
| ------------------------------------------ | ----------- |
| Base processing flow                       |🛠️|
| Modular component structure                |🛠️|
| Config/state separation                    |✅|
| Trailing sell logic                        |🛠️|
| Base allocation per step                   |-|
| Scaled allocation within price range       |-|
| Price-to-average padding logic             |-|
| Non-linear scaling curve                   |-|
| Dynamic base allocation growth             |-|
| Adaptive emphasis based on remaining funds |-|
| Step-based weight handling                 |-|
| Profit-based trailing stop logic           |✅|
| Minimum profit sell filter                 |-|
| Fund-aware aggression logic                |-|
