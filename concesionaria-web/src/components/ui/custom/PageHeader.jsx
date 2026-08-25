import { motion } from "framer-motion";

const PageHeader = ({
  title,
  icon: Icon,
  children,
}) => {
  return (
    <motion.div
      initial={{ opacity: 0, y: -8 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.3 }}
      className="relative mb-4 mt-2 flex min-h-13 items-center justify-between gap-4 overflow-hidden rounded-xl border border-slate-200/80 bg-white px-3 py-2.5 shadow-[0_4px_16px_rgba(15,23,42,0.07)] sm:px-4"
    >
      <span className="pointer-events-none absolute inset-x-0 bottom-0 h-[2px] bg-gradient-to-r from-[hsl(var(--nav-bg))] via-[hsl(var(--nav-bg)/0.45)] to-transparent" />

      <div
        className="
          flex min-w-0 flex-1 items-center justify-between gap-3
          max-sm:flex-wrap
        "
      >
        <div className="flex min-w-0 items-center gap-2.5">
          {Icon && (
            <div className="relative flex h-9 w-9 shrink-0 items-center justify-center rounded-xl bg-[hsl(var(--nav-bg)/0.09)] ring-1 ring-[hsl(var(--nav-bg)/0.14)]">
              <span className="absolute inset-1 rounded-lg bg-[hsl(var(--nav-bg))] shadow-[0_3px_8px_hsl(var(--nav-bg)/0.28)]" />
              <Icon className="relative text-white" size={15} strokeWidth={2.2} />
            </div>
          )}

          <h1 className="truncate text-base font-bold uppercase leading-none tracking-[0.035em] text-slate-900 sm:text-lg">
            {title}
          </h1>
        </div>

        {children && (
          <div
            className="
              ml-auto flex shrink-0 flex-wrap items-center justify-end gap-2
              max-sm:basis-full
            "
          >
            {children}
          </div>
        )}
      </div>
    </motion.div>
  );
};

export default PageHeader;
