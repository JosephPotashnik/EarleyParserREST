# EarleyParserREST

A powerful API providing access to an Earley Parser NuGET Library whose code is also available <a href="https://github.com/JosephPotashnik/EarleyParser"> here <a>. This API allows you to parse sentences according to <b>any</b> context free grammar.

## 📚 Documentation

<a href="http://josephpotashnik.github.io/EarleyParserREST/dist/index.html"> View the complete Swagger API Documentation</a>

## 🚀 Getting Started

### Main Endpoint: `/ParseSentence`

Send a POST request with a JSON object containing:

| Parameter | Type | Description |
|-----------|------|-------------|
| `grammarRules` | string[] | Non-lexicalized context-free grammar rules |
| `partOfSpeechRules` | string[] | Rules mapping parts of speech to lexical tokens |
| `sentence` | string | The sentence to parse (sequence of lexical tokens) |

### Response

The API returns an array of bracketed representations of the parsed trees.

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

## 🔍 What It Does

The API parses your input sentence according to the provided grammar and part-of-speech rules, returning all valid parse trees for the sentence.
