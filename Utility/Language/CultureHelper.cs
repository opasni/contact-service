namespace contact.Utility.Language;

public static class CultureHelper
{

  public const string EN = "en";
  public const string DE = "de";
  public const string SI = "si";

  public static readonly string[] Supported = new string[]
  {
    EN, DE, SI
  };

  public static string Default => EN;

  public static readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> Translatable = new()
  {
    { CultureHelper.EN, new()
      {
        { "subject", new()
          {
            { "freelance", "I need something done..." },
            { "offering-position", "Are you interested in working with us?" },
            { "technical-advice", "I need technical consultant?" },
            { "seeking-job", "I am searching for a job." },
            { "other", "Just want to chat" }
          }
        },
        { "feedback", new()
          {
            { "user", "Thank you for reaching out to me! I will get back to you as soon as possible!" },
            { "system", "<p>Received a new message from {{name}} {{email}}:</p><br><br><h1>{{subject}}</h1><br><p>{{message}}</p>" }
          }
        }
      }
    },
    { CultureHelper.DE, new()
      {
        { "subject", new()
          {
            { "freelance", "Ich benötige eine Dienstleistung..." },
            { "offering-position", "Sind Sie an einer Zusammenarbeit interessiert?" },
            { "technical-advice", "Ich benötige technische Beratung." },
            { "seeking-job", "Ich suche nach einer Stelle." },
            { "other", "Ich möchte nur plaudern." }
          }
        },
        { "feedback", new()
          {
            { "user", "Vielen Dank, dass Sie sich bei mir gemeldet haben! Ich werde mich so schnell wie möglich bei Ihnen melden!" },
            { "system", "<p>Nachricht erhalten von {{name}} {{email}}:</p><br><br><h1>{{subject}}</h1><br><p>{{message}}</p>" }
          }
        }
      }
    },
    { CultureHelper.SI, new()
      {
        { "subject", new()
          {
            { "freelance", "Potrebujem nekaj storjenega..." },
            { "offering-position", "Ste zainteresirani za sodelovanje z nami?" },
            { "technical-advice", "Potrebujem tehnično svetovanje." },
            { "seeking-job", "Iščem zaposlitev." },
            { "other", "Želim samo klepetati." }
          }
        },
        { "feedback", new()
          {
            { "user", "Hvala, ker ste stopili v stik z mano! Odgovoril vam bom v najkrajšem možnem času." },
            { "system", "<p>Prejel sporočilo od {{name}} {{email}}:</p><br><br><h1>{{subject}}</h1><br><p>{{message}}</p>" }
          }
        }
      }
    }
  };
}
