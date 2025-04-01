# 🌟 EarleyParserREST

A powerful Natural Language Processing API providing seamless access to an Earley Parser NuGET Library. Parse sentences according to **any** context-free grammar with this flexible and efficient implementation.

> 📦 **Source Code**: [GitHub Repository](https://github.com/JosephPotashnik/EarleyParser)

## 📚 Documentation

[View the complete Swagger API Documentation](http://josephpotashnik.github.io/EarleyParserREST/dist/index.html)

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

## 💻 Usage Example

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

## 🔎 Example Parse

For the sentence "the dog ran", you might receive a parse tree like:
```
[START [T1 [NP [D the] [N dog]] [VP [V0 ran]]]]
```

## 🔍 What It Does

The API applies the Earley parsing algorithm to your input sentence according to your provided grammar and part-of-speech rules. It handles ambiguity elegantly, returning all valid parse trees for the given sentence.

## 🛠 Advanced Use Cases

- Natural language understanding
- Syntax validation
- Linguistic research
- Custom language parsing
- Educational tools for grammar learning
