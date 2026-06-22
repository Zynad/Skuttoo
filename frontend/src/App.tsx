import { Route, Routes } from 'react-router-dom';
import { WorldMap } from './routes/WorldMap';
import { Island } from './routes/Island';
import { Exercise } from './routes/Exercise';
import { Profile } from './routes/Profile';
import { NotFound } from './routes/NotFound';

export function App() {
  return (
    <Routes>
      <Route path="/" element={<WorldMap />} />
      <Route path="/island/:key" element={<Island />} />
      <Route path="/exercise/:id" element={<Exercise />} />
      <Route path="/profile" element={<Profile />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
}
