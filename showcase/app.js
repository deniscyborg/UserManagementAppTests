// Live showcase data comes from the Allure report published alongside.
(() => {
  "use strict";

  const REPORT = "allure/";
  const MAX_RUNS = 20;
  const SERIES = [
    { key: "passed", label: "Passed" },
    { key: "failed", label: "Failed" },
    { key: "broken", label: "Broken" },
    { key: "skipped", label: "Skipped" },
  ];

  const $ = (id) => document.getElementById(id);

  async function fetchJson(path) {
    const response = await fetch(REPORT + path, { cache: "no-cache" });
    if (!response.ok) throw new Error(`${path}: ${response.status}`);
    return response.json();
  }

  function formatDuration(ms) {
    const seconds = Math.round(ms / 1000);
    return seconds < 60 ? `${seconds}s` : `${Math.floor(seconds / 60)}m ${seconds % 60}s`;
  }

  function formatDate(timestamp) {
    return new Date(timestamp).toLocaleString("en-GB", {
      day: "numeric",
      month: "short",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  }

  function renderSummary(summary) {
    const s = summary.statistic;
    const executed = s.total - s.skipped;
    const rate = executed > 0 ? (s.passed / executed) * 100 : 0;
    const allGreen = s.failed === 0 && s.broken === 0 && s.total > 0;

    $("stat-rate").textContent = `${rate.toFixed(rate % 1 === 0 ? 0 : 1)}%`;
    $("stat-passed").textContent = s.passed;
    $("stat-failed").textContent = s.failed;
    $("stat-broken").textContent = s.broken;
    $("stat-duration").textContent = formatDuration(summary.time.duration);

    const pill = $("stat-status");
    pill.textContent = allGreen ? "All tests passed" : `${s.failed + s.broken} not passing`;
    pill.classList.add(allGreen ? "ok" : "bad");

    $("run-meta").textContent = `${s.total} tests · finished ${formatDate(summary.time.stop)}`;
    $("stats").hidden = false;
  }

  function niceMax(value) {
    const step = value <= 10 ? 2 : value <= 50 ? 10 : 20;
    return Math.max(step, Math.ceil(value / step) * step);
  }

  function svg(tag, attrs) {
    const el = document.createElementNS("http://www.w3.org/2000/svg", tag);
    for (const [k, v] of Object.entries(attrs)) el.setAttribute(k, v);
    return el;
  }

  function runLabel(run, index) {
    return run.buildOrder ? `Run #${run.buildOrder}` : `Run ${index + 1}`;
  }

  // Bar with a rounded top (4px) and a flat bottom at the baseline
  function topRoundedRect(x, y, w, h, r) {
    const rr = Math.min(r, h, w / 2);
    return `M${x},${y + h} V${y + rr} Q${x},${y} ${x + rr},${y} H${x + w - rr} Q${x + w},${y} ${x + w},${y + rr} V${y + h} Z`;
  }

  function renderTrend(history) {
    const runs = history.slice(0, MAX_RUNS).reverse();
    if (runs.length === 0) return;

    const container = $("trend-chart");
    const width = Math.max(container.clientWidth, 280);
    const height = 220;
    const pad = { top: 8, right: 4, bottom: 22, left: 32 };
    const plotW = width - pad.left - pad.right;
    const plotH = height - pad.top - pad.bottom;
    const max = niceMax(Math.max(...runs.map((r) => r.data.total)));
    const slot = plotW / runs.length;
    const barW = Math.min(36, slot * 0.6);
    const gap = 2;

    const root = svg("svg", { viewBox: `0 0 ${width} ${height}`, role: "img", "aria-label": "Stacked columns of test results per run" });

    const ticks = 4;
    for (let i = 0; i <= ticks; i++) {
      const value = (max / ticks) * i;
      const y = pad.top + plotH - (value / max) * plotH;
      root.append(svg("line", { class: i === 0 ? "baseline" : "grid-line", x1: pad.left, x2: width - pad.right, y1: y, y2: y }));
      const label = svg("text", { class: "tick", x: pad.left - 8, y: y + 4, "text-anchor": "end" });
      label.textContent = Math.round(value);
      root.append(label);
    }

    const tooltip = $("trend-tooltip");
    const card = $("trend");

    runs.forEach((run, i) => {
      const x = pad.left + slot * i + (slot - barW) / 2;
      const group = svg("g", { class: "col" });
      let y = pad.top + plotH;
      const segments = SERIES.filter((s) => (run.data[s.key] || 0) > 0);

      segments.forEach((s, index) => {
        const h = (run.data[s.key] / max) * plotH;
        const isTop = index === segments.length - 1;
        // 2px background-coloured gap between stacked segments
        const drawH = Math.max(h - (isTop ? 0 : gap), 1);
        y -= h;
        const top = y + (isTop ? 0 : gap);
        const shape = isTop
          ? svg("path", { d: topRoundedRect(x, top, barW, drawH, 4) })
          : svg("rect", { x, y: top, width: barW, height: drawH });
        shape.setAttribute("fill", `var(--${s.key})`);
        group.append(shape);
      });

      if (i === runs.length - 1 || i === 0 || runs.length <= 8) {
        const label = svg("text", { class: "tick", x: x + barW / 2, y: height - 6, "text-anchor": "middle" });
        label.textContent = run.buildOrder ? `#${run.buildOrder}` : i === runs.length - 1 ? "latest" : "";
        root.append(label);
      }

      // Hover area wider than the bar
      const hit = svg("rect", { class: "hit", x: pad.left + slot * i, y: pad.top, width: slot, height: plotH, tabindex: 0 });
      const show = () => {
        root.querySelectorAll(".col").forEach((c) => c.classList.toggle("dim", c !== group));
        tooltip.innerHTML =
          `<strong>${runLabel(run, i)}${i === runs.length - 1 ? " · latest" : ""}</strong>` +
          SERIES.map(
            (s) => `<div class="row"><span><i class="key key-${s.key}"></i>${s.label}</span><b>${run.data[s.key] || 0}</b></div>`
          ).join("");
        tooltip.hidden = false;
        const cardRect = card.getBoundingClientRect();
        const hitRect = hit.getBoundingClientRect();
        const left = Math.min(hitRect.left - cardRect.left + hitRect.width / 2 + 8, cardRect.width - tooltip.offsetWidth - 8);
        tooltip.style.left = `${Math.max(8, left)}px`;
        tooltip.style.top = `${hitRect.top - cardRect.top}px`;
      };
      const hide = () => {
        root.querySelectorAll(".col").forEach((c) => c.classList.remove("dim"));
        tooltip.hidden = true;
      };
      hit.addEventListener("mouseenter", show);
      hit.addEventListener("focus", show);
      hit.addEventListener("mouseleave", hide);
      hit.addEventListener("blur", hide);

      root.append(group, hit);
    });

    container.replaceChildren(root);

    const tbody = $("trend-table").querySelector("tbody");
    tbody.replaceChildren(
      ...runs
        .slice()
        .reverse()
        .map((run, idx) => {
          const tr = document.createElement("tr");
          const cells = [runLabel(run, runs.length - 1 - idx), ...SERIES.map((s) => run.data[s.key] || 0), run.data.total];
          cells.forEach((c) => {
            const td = document.createElement("td");
            td.textContent = c;
            tr.append(td);
          });
          return tr;
        })
    );

    $("trend").hidden = false;
  }

  async function init() {
    try {
      const summary = await fetchJson("widgets/summary.json");
      renderSummary(summary);
    } catch {
      $("run-meta").textContent = "";
      $("status-error").hidden = false;
      return;
    }

    try {
      const history = await fetchJson("history/history-trend.json");
      $("trend").hidden = false; // container size is needed before drawing
      renderTrend(history);
      let resizeTimer;
      window.addEventListener("resize", () => {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(() => renderTrend(history), 150);
      });
    } catch {
      $("trend").hidden = true;
    }
  }

  init();
})();
