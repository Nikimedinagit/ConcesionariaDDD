import toast from "react-hot-toast";
import { createElement } from "react";

const baseStyle = {
  background: "rgba(248, 250, 252, 0.92)",
  color: "#0f172a",
  border: "1px solid rgba(226, 232, 240, 0.8)",
  boxShadow: "0 10px 25px -5px rgba(0,0,0,0.15)",
  backdropFilter: "blur(10px)",
  borderRadius: "12px",
  padding: "12px 14px",
  minWidth: "260px",
};

const colors = {
  success: "#22c55e",
  error: "#ef4444",
  warning: "#f59e0b",
  info: "#3b82f6",
};

const renderMessage = (message, description) =>
  createElement(
    "div",
    { style: { display: "flex", flexDirection: "column", gap: "4px" } },
    createElement(
      "div",
      {
        style: {
          fontWeight: 700,
          fontSize: "16px",
          color: "#0f172a",
        },
      },
      message
    ),
    description &&
      createElement(
        "div",
        {
          style: {
            fontWeight: 400,
            fontSize: "14px",
            color: "#475569",
          },
        },
        description
      )
  );

const show = (type, message, options = {}) => {
  const fn = toast[type] || toast;

  fn(renderMessage(message, options.description), {
    style: baseStyle,
    iconTheme: {
      primary: colors[type] || "#3b82f6",
      secondary: "#f8fafc",
    },
  });
};

export const toastService = {
  success: (message, options) => show("success", message, options),
  error: (message, options) => show("error", message, options),
  warning: (message, options) => show("loading", message, options), // fallback visual
  info: (message, options) => show("blank", message, options),
};