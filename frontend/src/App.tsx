import { Route, Routes, useLocation } from 'react-router-dom';
import { AnimatePresence, motion, useReducedMotion } from 'framer-motion';
import { WorldMap } from './routes/WorldMap';
import { Island } from './routes/Island';
import { Exercise } from './routes/Exercise';
import { Profile } from './routes/Profile';
import { NotFound } from './routes/NotFound';

export function App() {
  const location = useLocation();
  const reduce = useReducedMotion();

  // Gentle page transition between map ↔ island ↔ exercise. When the child (or OS) prefers
  // reduced motion, collapse to a quick cross-fade with no movement.
  const variants = reduce
    ? { initial: { opacity: 0 }, animate: { opacity: 1 }, exit: { opacity: 0 } }
    : { initial: { opacity: 0, y: 14 }, animate: { opacity: 1, y: 0 }, exit: { opacity: 0, y: -14 } };

  return (
    <AnimatePresence mode="wait">
      <motion.div
        key={location.pathname}
        className="min-h-full"
        variants={variants}
        initial="initial"
        animate="animate"
        exit="exit"
        transition={{ duration: reduce ? 0.12 : 0.22, ease: 'easeOut' }}
      >
        <Routes location={location}>
          <Route path="/" element={<WorldMap />} />
          <Route path="/island/:key" element={<Island />} />
          <Route path="/exercise/:id" element={<Exercise />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </motion.div>
    </AnimatePresence>
  );
}
