
// This class contains metadata for your submission. It plugs into some of our
// grading tools to extract your game/team details. Ensure all Gradescope tests
// pass when submitting, as these do some basic checks of this file.
public static class SubmissionInfo
{
    // TASK: Fill out all team + team member details below by replacing the
    // content of the strings. Also ensure you read the specification carefully
    // for extra details related to use of this file.

    // URL to your group's project 2 repository on GitHub.
    public static readonly string RepoURL = "https://github.com/COMP30019/project-2-sandwich-computing-studios";
    
    // Come up with a team name below (plain text, no more than 50 chars).
    public static readonly string TeamName = "Sandwich Computing Studios";
    
    // List every team member below. Ensure student names/emails match official
    // UniMelb records exactly (e.g. avoid nicknames or aliases).
    public static readonly TeamMember[] Team = new[]
    {
        // new TeamMember("Lucas Wong", "lucaskw@student.unimelb.edu.au"), // Dropped out
        new TeamMember("Benjamin Coyne", "Bacoyne@student.unimelb.edu.au"),
        new TeamMember("Yu-Tien Huang", "yutienh@student.unimelb.edu.au"),
        new TeamMember("Simai Yu", "simaiy@student.unimelb.edu.au"),
        // Remove the following line if you have a group of 3
        new TeamMember("Alex Ozoline", "aozoline@student.unimelb.edu.au"), 
    };

    // This may be a "working title" to begin with, but ensure it is final by
    // the video milestone deadline (plain text, no more than 50 chars).
    public static readonly string GameName = "Blockade Runners";

    // Write a brief blurb of your game, no more than 200 words. Again, ensure
    // this is final by the video milestone deadline.
    public static readonly string GameBlurb = 
@"You have been entrusted with carrying valuable cargo to a rendezvous point in the outer systems.
The nature of your cargo has attracted bandits and scavengers who would take it from you.
Navigate through the asteroid field, while fighting off various enemies, as you see how far you can make it before falling.

This 2D, top-down game puts you at the controls of a ship flying through a randomized obstacle course, where you will come face-to-face with enemies.
As you play the challenge will increase, the asteroids will fly faster, enemies will hit harder.
You can customize your ship to match your play style, swapping out your engines and weapons for different stats and gameplay experiences. 
";
    
    // By the gameplay video milestone deadline this should be a direct link
    // to a YouTube video upload containing your video. Ensure "Made for kids"
    // is turned off in the video settings. 
    public static readonly string GameplayVideo = "https://www.youtube.com/watch?v=sWmVaA_OC5U";
    
    // No more info to fill out!
    // Please don't modify anything below here.
    public readonly struct TeamMember
    {
        public TeamMember(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }
}
