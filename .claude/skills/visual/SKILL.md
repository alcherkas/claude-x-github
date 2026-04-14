---
name: visual
description: Visual review of Vercel preview deployment using Playwright
disable-model-invocation: true
---

# Visual Review of Vercel Preview

Perform a visual review of the Vercel preview deployment for this PR.

## Steps

1. **Find the Vercel preview URL** for this PR:
   ```bash
   gh pr checks $PR_NUMBER --json name,state,link --jq '.[] | select(.name | test("vercel|Preview"; "i")) | .link'
   ```
   If no URL is found via checks, try the deployments API:
   ```bash
   gh api repos/$REPO/deployments?environment=Preview --jq '.[0].payload.web_url // .[0].environment_url'
   ```

2. **Wait for the deployment to be ready.** Poll until the preview URL returns HTTP 200 (retry up to 60 seconds):
   ```bash
   for i in $(seq 1 12); do curl -s -o /dev/null -w "%{http_code}" "$PREVIEW_URL" | grep -q 200 && break || sleep 5; done
   ```

3. **Use Playwright MCP to visually review the site:**
   - Navigate to the preview URL with `mcp__playwright__browser_navigate`
   - Take a screenshot with `mcp__playwright__browser_take_screenshot`
   - Get the page accessibility snapshot with `mcp__playwright__browser_snapshot`
   - Navigate to other key routes (e.g. `/health`, `/weatherforecast`) and screenshot those too

4. **Analyze and report findings.** Look for:
   - Broken layouts or missing elements
   - Console errors or failed network requests
   - Visual regressions compared to what the code intends
   - Accessibility issues visible in the snapshot

5. **Post findings as a PR comment** using `gh pr comment` with a summary of what was reviewed and any issues found. Include screenshots if possible.
