# Claude × GitHub — Demo .NET 10 Pipeline

A demo .NET 10 Web API application with CI/CD pipelines for build, test, and AI-powered code review using [Claude Code Action](https://github.com/anthropics/claude-code-action).

## Project Structure

```
├── .github/workflows/
│   ├── ci.yml              # Build & Test pipeline
│   └── claude-code.yml     # Claude Code AI review pipeline
├── src/DemoApi/            # .NET 10 Web API
├── tests/DemoApi.Tests/    # Unit & integration tests (xUnit)
└── DemoApi.slnx            # Solution file
```

## API Endpoints

| Endpoint             | Description             |
|----------------------|-------------------------|
| `GET /`              | Welcome message         |
| `GET /weatherforecast` | Random weather forecasts |
| `GET /health`        | Health check             |

## Local Development

```bash
# Build
dotnet build

# Run
dotnet run --project src/DemoApi

# Test
dotnet test
```

## CI/CD Pipelines

### 1. Build & Test (`ci.yml`)

Runs on every push and PR to `main`. Steps:
- Setup .NET 10 SDK
- Restore, build, and run all tests

### 2. Claude Code Review (`claude-code.yml`)

AI-powered code review using Anthropic's Claude Code Action. Triggers on:
- **Pull request opened/updated** — automatic review
- **Issue comment** — on-demand when a comment contains `@claude`

### Setup: `CLAUDE_CODE_OAUTH_TOKEN`

1. Go to **Settings → Secrets and variables → Actions** in your GitHub repository.
2. Click **New repository secret**.
3. Name: `CLAUDE_CODE_OAUTH_TOKEN`
4. Value: Your Anthropic API key or OAuth token.
5. Click **Add secret**.

The Claude Code workflow uses this secret to authenticate with the Anthropic API:

```yaml
- uses: anthropics/claude-code-action@v1
  with:
    anthropic_api_key: ${{ secrets.CLAUDE_CODE_OAUTH_TOKEN }}
```

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- GitHub repository with `CLAUDE_CODE_OAUTH_TOKEN` secret configured
