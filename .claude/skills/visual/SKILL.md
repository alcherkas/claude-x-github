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

4. **Save and commit screenshots to the PR branch:**
   ```bash
   mkdir -p screenshots
   # After each screenshot, save it to screenshots/ directory
   git add screenshots/
   git commit -m "chore: add visual review screenshots"
   git push
   ```

5. **Post findings as a PR comment** using `gh pr comment`. Embed screenshots inline using raw GitHub URLs:
   ```
   ![Homepage](https://raw.githubusercontent.com/OWNER/REPO/BRANCH/screenshots/homepage.png)
   ```
   Replace OWNER, REPO, and BRANCH with actual values from the environment.

   The comment should include:
   - Screenshots of each page reviewed
   - Any visual issues found (broken layouts, missing elements, console errors)
   - Accessibility issues visible in the snapshot
   - Overall assessment
