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
      className="
        mb-4 mt-2
        flex min-h-11 items-center justify-between gap-3
        rounded-lg
        border border-slate-200/70
        bg-white
        px-3 py-2 sm:px-4
        shadow-sm
      "
    >
      <div
        className="
          flex min-w-0 flex-1 items-center justify-between gap-3
          max-sm:flex-wrap
        "
      >
        <div className="flex min-w-0 items-center gap-2.5">
          {Icon && (
            <div
              className="
                flex h-7 w-7 shrink-0
                items-center justify-center
                rounded-lg
                bg-[hsl(var(--nav-bg))]
                text-white
              "
            >
              <Icon size={14} strokeWidth={2.1} />
            </div>
          )}

          <h1
            className="
              truncate text-lg font-semibold leading-tight
              tracking-tight text-slate-900 sm:text-xl
            "
          >
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
