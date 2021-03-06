import React from 'react';
import ControlTypesReact from './react/ControlTypes';
import ControlTypesKo from './knockout/ControlTypes';
import Example from './components/Example';

export default _ => <Example vm="ControlTypesExample" react={<ControlTypesReact />} ko={<ControlTypesKo />} />;
