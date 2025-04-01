# 🌟 EarleyParserREST

A powerful Natural Language Processing API providing seamless access to an Earley Parser NuGET Library. Parse sentences according to **any** context-free grammar with this flexible and efficient implementation.

> 📦 **Source Code**: [GitHub Repository](https://github.com/JosephPotashnik/EarleyParser)

## 📚 Documentation

[View the complete Swagger API Documentation](http://josephpotashnik.github.io/EarleyParserREST/dist/index.html)

## 💍 Cloning the Repository

To get a copy of the project locally, run the following command:

```sh
git clone https://github.com/JosephPotashnik/EarleyParserREST.git
cd EarleyParserREST
```
## ⚙️ Running the Project Locally

### Prerequisites

Ensure you have the following installed:
- **.NET SDK** (Latest stable version) → [Download here](https://dotnet.microsoft.com/download)
- **Docker** (Optional, if running in a container) → [Download here](https://www.docker.com/get-started)

### Running with .NET CLI

```sh
cd EarleyParserRest
# Restore dependencies
dotnet restore
# Build the project
dotnet build
# Run the API
dotnet run
```
ASP.NET assigns ports dynamically, typically API would be accessible at `http://localhost:5000` (or `http://localhost:5001` for HTTPS). However, once the project runs, the console logs the actual ports in use.

### Running with Docker

Alternatively, you can run the API in a Docker container. 

```sh
docker build -t earleyparserrest .
docker run -p 5000:5000 earleyparserrest
```
## 🚀 Getting Started

### Main Endpoint: `/ParseSentence`

Send a POST request with a JSON object containing:

| Parameter | Type | Description |
|-----------|------|-------------|
| `grammarRules` | string[] | Non-lexicalized context-free grammar rules |
| `partOfSpeechRules` | string[] | Rules mapping parts of speech to lexical tokens |
| `sentence` | string | The sentence to parse (sequence of lexical tokens) |

## 📋 Parameter Formats

### Grammar Rules Format

Each string in the `grammarRules` array must follow this format:
```
"X -> Y1 Y2 Y3 ..."
```

Where:
- `X` is a single left-hand side nonterminal
- `->` is the production symbol
- `Y1 Y2 Y3 ...` is a sequence of right-hand side nonterminals (one or more)

#### Example Grammar Rules
```json
[
  "START -> T1",
  "T1 -> NP VP",
  "VP -> V0",
  "VP -> V1 NP",
  "VP -> V2 PP", 
  "VP -> V3 T1",
  "PP -> P NP",
  "NP -> D N",
  "NP -> PN",
  "NP -> NP PP",
  "VP -> VP PP",
  "NP -> D NBAR",
  "NBAR -> A NBAR",
  "NBAR -> A N"
]
```

### Part of Speech Rules Format

Each string in the `partOfSpeechRules` array must follow this format:
```
"X -> token"
```

Where:
- `X` is a single left-hand side nonterminal (the part of speech)
- `->` is the production symbol
- `token` is the lexical token

#### Vocabulary Optimization
**Important:** There is no need to send the entire vocabulary with each request. It suffices to send only the Part of Speech rules involving the lexical tokens present in the sentence being parsed. This optimization significantly reduces request payload size.

#### Lexical Ambiguity
A lexical token can correspond to more than one part of speech. For example:
```json
[
  "N -> saw",
  "V -> saw"
]
```

The parser handles this syntactic ambiguity naturally. However, note that it has no semantic component, so it cannot distinguish between different meanings of the same word (e.g., 'bank' as a river bank versus 'bank' as a financial institution).

#### Example Part of Speech Rules
```json
[
  "PN -> John",
  "PN -> Mary",
  "V1 -> loved",
  "V1 -> saw",
  "V0 -> fell",
  "V0 -> ran",
  "D -> the",
  "D -> a",
  "N -> man",
  "N -> woman",
  "P -> with",
  "P -> to",
  "A -> pretty",
  "A -> big"
]
```

## 📊 Response

The API returns an array of bracketed representations of the parsed trees. Multiple parse trees may be returned if the sentence is ambiguous according to your grammar.

## 🧩 Usage Example: PP-Attachment Ambiguity

Below is a classic example demonstrating how the parser handles prepositional phrase attachment ambiguity:

```javascript
// Sentence to parse
const sentence = "John saw the girl with the telescope";

// Grammar rules defining syntactic structure
const grammarRules = [
  "START -> T1",
  "T1 -> NP VP",
  "VP -> V0",
  "VP -> V1 NP",
  "VP -> V2 PP", 
  "VP -> V3 T1",
  "PP -> P NP",
  "NP -> D N",
  "NP -> PN",
  "NP -> NP PP",
  "VP -> VP PP"
];

// Only including the parts of speech needed for this sentence
const partOfSpeechRules = [
  "PN -> John",
  "V1 -> saw",
  "D -> the",
  "N -> girl",
  "N -> telescope",
  "P -> with"
];

// The API returns two possible parses:
[
  // Parse 1: VP attachment - John [saw the girl] [with the telescope]
  "(START (T1 (NP (PN John)) (VP (VP (V1 saw) (NP (D the) (N girl))) (PP (P with) (NP (D the) (N telescope))))))",
  
  // Parse 2: NP attachment - John saw [the girl [with the telescope]]
  "(START (T1 (NP (PN John)) (VP (V1 saw) (NP (NP (D the) (N girl)) (PP (P with) (NP (D the) (N telescope)))))))"
]
```

This example illustrates how the parser correctly identifies both interpretations:
1. John used a telescope to see the girl (VP attachment)
2. John saw a girl who had a telescope (NP attachment)

## 💻 API Implementation

Here's how to call the API in TypeScript:

```typescript
async function parseSentence(
  sentence: string, 
  grammarRules: string[], 
  POSRules: string[]
): Promise<string[]> {
  
  // Prepare request body
  const bodyData = {
    GrammarRules: grammarRules,
    PartOfSpeechRules: POSRules,
    Sentence: sentence
  };

  // Send request to API
  const response = await fetch(`${API_URL}/ParseSentence/`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(bodyData) 
  });

  // Parse and return response
  const data = await response.json() as string[];
  return data;
}
```

## 🔍 How It Works

The API applies the Earley parsing algorithm to your input sentence according to your provided grammar and part-of-speech rules. The algorithm:

1. Processes the input tokens from left to right
2. Builds sets of partial parses (chart entries)
3. Efficiently handles ambiguity by exploring all valid interpretations
4. Returns all possible parse trees that conform to your grammar

This makes it particularly effective for natural language processing where ambiguity is common and multiple interpretations may be valid.

## 🛠 Advanced Use Cases

- Natural language understanding
- Syntax validation
- Linguistic research
- Grammar checking
- Custom language parsing
- Educational tools for grammar learning
- Automated text analysis
