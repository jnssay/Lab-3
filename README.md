Geometry Dash?

### AudioMixer
To manage Music in the background and SFX for jumping
Two types of BGM added
Slight Duck Volume effect added to dim music while jumping

### Observer Pattern
GameManager class that manages key game events
Usage of UnityEvent to define certain events such as OnGameOver and OnScoreChanged
Observer setup in HUDManager (eg. GameManager.Instance.OnScoreChanged.AddListener(UpdateScore))
Trigger setup (eg. OnScoreChanged.Invoke(newScore)) 

### Scoring System
Easy mode: Score is added when player successfully jumps over an obstacle
“Hard” mode: Score is added each time player lands on a new platform
Game ends if player collides with obstacle or falls through the platforms
